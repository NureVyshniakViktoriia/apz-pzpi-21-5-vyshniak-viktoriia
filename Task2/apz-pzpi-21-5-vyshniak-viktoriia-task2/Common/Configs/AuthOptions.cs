﻿using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Common.Configs;
public class AuthOptions
{
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string Secret { get; set; }

    public int TokenLifetime { get; set; }

    public int RefreshTokenLifetime { get; set; }

    public SymmetricSecurityKey SymmetricSecurityKey
        => new(Encoding.ASCII.GetBytes(Secret));
}
