using tutorial9.DTOs;

namespace tutorial9.Services;

public interface IAuthService
{
    void RegisterUser(RegisterUserDto request);
    (string accessToken, string refreshToken) LoginUser(LoginDto request);
}