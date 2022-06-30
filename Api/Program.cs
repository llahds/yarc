using Api.Data;
using Api.Services;
using Api.Services.Authentication;
using Api.Services.Comments;
using Api.Services.Forums;
using Api.Services.FullText;
using Api.Services.Moderation;
using Api.Services.Posts;
using Api.Services.Reporting;
using Api.Services.Scoring;
using Api.Services.Text.Toxicity;
using Api.Services.Users;
using AutoMapper;
using Hangfire;
using Lucene.Net.Store;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

Task.Delay(10_000).Wait();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IModerationService, ModerationService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ICommentService, CommentService>();
builder.Services.AddTransient<IForumService, ForumService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IPostViewService, PostViewService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IReportingService, ReportingService>();
builder.Services.AddTransient<IPasswordHashService, PasswordHashService>();
builder.Services.AddTransient<IUpdatePostScores, UpdatePostScores>();

builder.Services.AddSingleton<IFullTextIndex>(new FullTextIndex(FSDirectory.Open(builder.Configuration["connectionStrings:fts"])));
builder.Services.AddSingleton<IToxicityService, ToxicityService>();

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration["connectionStrings:db"]));
builder.Services.AddHangfireServer();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.SetIsOriginAllowed(_ => true)
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
}));
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Use the authentication endpoint (/api/1.0/authentication) to generate an access token.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<YARCContext>(options => options.UseSqlServer(builder.Configuration["connectionStrings:db"]));
var key = Encoding.ASCII.GetBytes(builder.Configuration["authentication:secretKey"]);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        LifetimeValidator = new LifetimeValidator((DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
        {
            return expires > DateTime.UtcNow;
        })
    };
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Signalr passes the token as a query string
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/real-time")
                || path.StartsWithSegments("/api/1.0/attachments")))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("Administrator", policy => policy.RequireClaim("Role", "Administrator"));
});


var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});

var mapper = config.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddSpaStaticFiles(cfg => cfg.RootPath = "wwwroot");

var app = builder.Build();

app.UseSpaStaticFiles();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHangfireDashboard();
app.UseSpa(spa => spa.Options.SourcePath = "wwwroot");

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<YARCContext>();
    dataContext.Database.Migrate();
}

RecurringJob.AddOrUpdate<IUpdatePostScores>("UpdatePostScores", x => x.Execute(), Cron.MinuteInterval(3));

app.Run();

