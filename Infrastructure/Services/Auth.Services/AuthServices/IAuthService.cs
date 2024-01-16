namespace Infrastructure
{
    public interface IAuthService
    {
        Task<Response<LoginResponse>> LoginAsync(LoginRequest request);
        Task<Response<RegisterResponse>> RegisterAsync(RegisterRequest request);
    }
}
