using Api.Data;
using Api.Services.FullText;
using Api.Services.FullText.Documents;
using Lucene.Net.Store;
using Microsoft.EntityFrameworkCore;

var connectionstring = "Server=.;Database=YARC;Trusted_Connection=True;";

var optionsBuilder = new DbContextOptionsBuilder<YARCContext>();
optionsBuilder.UseSqlServer(connectionstring);

using (var index = new FullTextIndex(FSDirectory.Open("D:\\Development\\Attack of the Clones\\Reddit\\data\\fts")))
{
    using (var context = new YARCContext(optionsBuilder.Options))
    {
        var i = 0;

        var forums = new Dictionary<int, string>();

        foreach (var forum in context.Forums)
        {
            index.Save(new ForumFTS
             {
                 Id = forum.Id,
                 Name = forum.Name,
                 Description = forum.Description,
                 Slug = forum.Slug
             });

            i++;

            forums.Add(forum.Id, forum.Slug);

            if (i % 100 == 0)
            {
                Console.WriteLine($"{i} forums indexed");
            }
        }

        i = 0;

        foreach (var post in context.Posts)
        {
            index.Save(new PostFTS
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                ForumName = forums[post.ForumId]
            });

            i++;

            if (i % 100 == 0)
            {
                Console.WriteLine($"{i} posts indexed");
            }
        }

        i = 0;

        foreach (var comment in context.Comments)
        {
            index.Save(new CommentFTS
            {
                Id = comment.Id,
                Text = comment.Text
            });

            i++;

            if (i % 100 == 0)
            {
                Console.WriteLine($"{i} comments indexed");
            }
        }
    }

    index.Commit();
}
