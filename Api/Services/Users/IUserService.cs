using Api.Models;

namespace Api.Services.Users
{
    public interface IUserService
    {
        Task<bool> EmailAlreadyExists(string email, int? userId);
        Task<UserSettingsModel> GetUserSettings();
        Task<AuthenticationTokenModel> Register(RegisterModel model);
        Task UpdateEmail(string email);
        Task UpdatePassword(string password);
        Task UpdateUserName(string userName);
        Task UpdateUserSettings(UserSettingsModel model);
        Task<bool> UserNameAlreadyExists(string userName, int? userId);
    }
}
