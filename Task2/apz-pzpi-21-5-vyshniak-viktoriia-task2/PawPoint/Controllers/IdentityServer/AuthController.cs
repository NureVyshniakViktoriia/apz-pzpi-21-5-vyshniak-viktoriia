using BLL.Contracts;
using BLL.Infrastructure.Models.User;
using Common.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPI.Infrastructure.Extensions;
using WebAPI.Infrastructure.Models;

namespace WebAPI.Controllers.IdentityServer;
[Area("auth")]
[Route("api/[area]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthOptions _authOptions;
    private readonly IUserService _userService;

    public AuthController(
        AuthOptions authOptions,
        IUserService userService)
    {
        _authOptions = authOptions;
        _userService = userService;
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody]LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = _userService.LoginUser(model.Login, model.Password);
        var token = GetToken(userId);

        return Ok(token);
    }

    [HttpGet("get-user-by-refresh-token")]
    public ActionResult GetUserByRefresh([FromHeader]string refreshTokenString)
    {
        var refreshToken = refreshTokenString.DecodeToken();
        var user = _userService.GetUserByRefreshToken(refreshToken);
        var token = GetToken(user);

        return Ok(token);
    }

    #region Helpers

    private TokenModel GetToken(UserModel user)
    {
        var authParams = _authOptions;
        var securityKey = authParams.SymmetricSecurityKey;
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userRole = user.Role.ToString();

        var jwtToken = new JwtSecurityToken(
            authParams.Issuer,
            authParams.Audience,
            new List<Claim>() {
                    new Claim("id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, userRole),
                    new Claim("role", userRole),
            },
            expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        var token = new TokenModel()
        {
            AccessToken = tokenString,
            RefreshToken = _userService.CreateRefreshToken(user.UserId).EncodeToken()
        };

        return token;
    }

    #endregion
}
