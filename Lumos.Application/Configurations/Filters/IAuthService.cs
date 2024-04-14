namespace NutriPro.Application.Configurations.Filters
{
    public interface IAuthService : ITransientDependency
    {
        bool ValidateToken(string token);
        string GenerateJwtToken(string email);
    }
}
