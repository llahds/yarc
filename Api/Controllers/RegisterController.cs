using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    public class RegisterController : Controller
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly ITokenGeneratorService tokens;

        public RegisterController(
            YARCContext context,
            IMapper mapper,
            ITokenGeneratorService tokens)
        {
            this.context = context;
            this.mapper = mapper;
            this.tokens = tokens;
        }

        [HttpPost, Route("api/1.0/register")]
        [ProducesResponseType(200, Type = typeof(AuthenticationTokenModel))]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var emailAlreadyExists = await context
                .Users
                .AnyAsync(U => U.Email == model.Email);

            if (emailAlreadyExists)
            {
                this.ModelState.AddModelError(nameof(model.Email), "Email already exists.");
            }

            var userNameAlreadyExists = await context
                .Users
                .AnyAsync(U => U.UserName == model.UserName);

            if (userNameAlreadyExists)
            {
                this.ModelState.AddModelError(nameof(model.UserName), "User name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = this.mapper.Map<User>(model);

            await this.context.Users.AddAsync(entity);

            await this.context.SaveChangesAsync();

            var upn = new Claim(ClaimTypes.NameIdentifier, entity.UserName);
            var token = await this.tokens.Generate(new[] { upn });

            return this.Ok(new AuthenticationTokenModel
            {
                UserName = model.UserName,
                Token = token
            });
        }
    }
}
