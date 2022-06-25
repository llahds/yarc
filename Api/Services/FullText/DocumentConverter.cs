using Lucene.Net.Documents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Api.Services.FullText
{
    public class DocumentConverter
    {
        private readonly Dictionary<Type, Func<object, FullTextIndex.IndexRequest>> converters = new Dictionary<Type, Func<object, FullTextIndex.IndexRequest>>();

        public DocumentConverter()
        {

        }

        private Func<object, FullTextIndex.IndexRequest> Build<TEntity>()
        {
            var type = typeof(TEntity);

            var getters = new List<Tuple<string, FieldType, Func<TEntity, object>>>();

            getters.AddRange(type
                .GetProperties()
                .Where(P => P.GetCustomAttributes<IdAttribute>().Any())
                .Select(P => new Tuple<string, FieldType, Func<TEntity, object>>(P.Name, FieldType.Id, Convert<TEntity>(P)))
                .ToArray());

            getters.AddRange(type
                .GetProperties()
                .Where(P => P.GetCustomAttributes<StringFieldAttribute>().Any())
                .Select(P => new Tuple<string, FieldType, Func<TEntity, object>>(P.Name, FieldType.StringField, Convert<TEntity>(P)))
                .ToArray());

            getters.AddRange(type
                .GetProperties()
                .Where(P => P.GetCustomAttributes<TextFieldAttribute>().Any())
                .Select(P => new Tuple<string, FieldType, Func<TEntity, object>>(P.Name, FieldType.TextField, Convert<TEntity>(P)))
                .ToArray());

            getters.AddRange(type
                .GetProperties()
                .Where(P => P.GetCustomAttributes<Int64FieldAttribute>().Any())
                .Select(P => new Tuple<string, FieldType, Func<TEntity, object>>(P.Name, FieldType.Int64Field, Convert<TEntity>(P)))
                .ToArray());

            getters.AddRange(type
                .GetProperties()
                .Where(P => P.GetCustomAttributes<Int32FieldAttribute>().Any())
                .Select(P => new Tuple<string, FieldType, Func<TEntity, object>>(P.Name, FieldType.Int32Field, Convert<TEntity>(P)))
                .ToArray());

            return new Func<object, FullTextIndex.IndexRequest>(entity =>
            {
                var values = getters
                    .Select(G => new
                    {
                        Name = G.Item1,
                        Type = G.Item2,
                        Value = G.Item3((TEntity)entity)
                    })
                    .ToArray();

                var document = new Document();
                
                document.AddStringField("__type", typeof(TEntity).ToString(), Field.Store.YES);
                document.AddStoredField("__content", JsonConvert.SerializeObject(entity));

                string id = "";

                var buffer = new StringBuilder();
                foreach (var value in values)
                {
                    if (value.Type == FieldType.Id)
                    {
                        id = $"{typeof(TEntity).ToString()}-{value.Value.ToString()}";
                        document.AddStringField("__id", id, Field.Store.NO);
                        document.AddStringField(value.Name, value.Value.ToString(), Field.Store.NO);
                    }
                    else if (value.Type == FieldType.Int64Field)
                    {
                        document.AddInt64Field(value.Name, (long)value.Value, Field.Store.NO);
                    }
                    else if (value.Type == FieldType.TextField && value.Value != null)
                    {
                        if (value.Value is string[])
                        {
                            foreach (var v in (string[])value.Value)
                            {
                                document.AddTextField(value.Name, v, Field.Store.NO);
                                buffer.Append($"\r{v}");
                            }
                        }
                        else
                        {
                            document.AddTextField(value.Name, value.Value.ToString(), Field.Store.NO);
                            buffer.Append($"\r{value.Value.ToString()}");
                        }
                    }
                    else if (value.Type == FieldType.StringField && value.Value != null)
                    {
                        if (value.Value is string[])
                        {
                            foreach (var v in (string[])value.Value)
                            {
                                document.AddStringField(value.Name, v, Field.Store.NO);
                                buffer.Append($"\r{v}");
                            }
                        }
                        else
                        {
                            document.AddStringField(value.Name, value.Value.ToString(), Field.Store.NO);
                            buffer.Append($"\r{value.Value.ToString()}");
                        }
                    }
                    else if (value.Type == FieldType.Int32Field)
                    {
                        document.AddInt32Field(value.Name, (int)value.Value, Field.Store.NO);
                    }
                }
                document.AddTextField("__text", buffer.ToString(), Field.Store.NO);

                return new FullTextIndex.IndexRequest
                {
                    Id = id,
                    Document = document
                };
            });
        }

        private Func<object, FullTextIndex.IndexRequest> GetConverter<TEntity>()
        {
            lock (this.converters)
            {
                if (this.converters.ContainsKey(typeof(TEntity)))
                {
                    return this.converters[typeof(TEntity)];
                }
                else
                {
                    var converter = this.Build<TEntity>();
                    this.converters.Add(typeof(TEntity), converter);
                    return converter;
                }
            }
        }

        private Func<TEntity, object> Convert<TEntity>(PropertyInfo propertyInfo)
        {
            var getter = propertyInfo.GetGetMethod();
            var entity = Expression.Parameter(typeof(TEntity));
            var getterCall = Expression.Call(entity, getter);
            var castToObject = Expression.Convert(getterCall, typeof(object));
            var lambda = Expression.Lambda(castToObject, entity);
            return (Func<TEntity, object>)lambda.Compile();
        }

        public FullTextIndex.IndexRequest Convert<TEntity>(TEntity entity)
        {
            return this.GetConverter<TEntity>()(entity);
        }
    }
}
