using BankingSystem.Core.Entities;
using System.Security.Claims;


namespace BankingSystem.Service.ServiceInterfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
