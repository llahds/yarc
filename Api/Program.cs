using Api.Data;
using Api.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});
builder.Services.AddDbContext<YARCContext>(options => options.UseSqlServer(builder.Configuration["connectionStrings:db"]));

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});

var mapper = config.CreateMapper();

builder.Services.AddSingleton(mapper);

var app = builder.Build();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
