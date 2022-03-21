using System;
using Microsoft.IdentityModel.Tokens;

namespace Web.Auth
{
  public class JwtIssuerOptions
  {
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public DateTime Expiration => IssuedAt.Add(ValidFor);

    public DateTime NotBefore => DateTime.UtcNow;

    private DateTime IssuedAt => DateTime.UtcNow;
    private TimeSpan ValidFor { get; } = TimeSpan.FromMinutes(15);

    public SigningCredentials SigningCredentials { get; set; }
  }
}
