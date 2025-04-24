using Auditt.Application.Domain.Entities;
using SNET.Framework.Domain.Authentications.Jwt;

namespace Auditt.Domain.Authentications
{
    public interface IManagerToken
    {
        public TokenModel GenerateToken(User user);
        public string ValidateToken(string token);
    }
}
