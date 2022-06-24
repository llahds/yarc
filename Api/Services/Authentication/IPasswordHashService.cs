namespace Api.Services.Authentication
{
    public interface IPasswordHashService
    {
        Task<string> Generate(string password);
        Task<bool> Validate(string password, string passwordHash);
    }
}