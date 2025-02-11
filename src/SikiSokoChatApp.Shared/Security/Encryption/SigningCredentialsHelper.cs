using Microsoft.IdentityModel.Tokens;

namespace SikiSokoChatApp.Shared.Security.Encryption;

/// <summary>
/// Provides methods to create signing credentials.
/// </summary>
public static class SigningCredentialsHelper
{
    public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey) =>
        new(securityKey, SecurityAlgorithms.HmacSha512Signature);
}