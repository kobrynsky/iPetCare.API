using Domain;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(ApplicationUser user);
    }
}