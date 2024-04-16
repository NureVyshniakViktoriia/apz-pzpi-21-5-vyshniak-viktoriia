using BLL.Contracts;
using BLL.Infrastructure.Models.User;
using Common.Resources;
using DAL.Infrastructure.Models;
using DAL.Infrastructure.Models.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Infrastructure.Models;

namespace WebAPI.Controllers;
[Area("user")]
[Route("api/[area]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IEmailService> _emailService;

    public UserController(
        Lazy<IUserService> userService,
        Lazy<IEmailService> emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public ActionResult Register([FromBody] RegisterUserModel registerModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _userService.Value.RegisterUser(registerModel);

        return Ok();
    }

    [HttpPost("forgot-password")]
    public ActionResult ForgotPassword([FromBody] ForgotPasswordModel forgotPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _emailService.Value.SendResetPasswordEmail(forgotPassword.Email);

        return Ok();
    }

    [HttpGet("request-reset-password")]
    public ActionResult RequestResetPassword([FromQuery] string token)
    {
        var isTokenValid = _userService.Value.IsResetPasswordTokenValid(token);

        if (isTokenValid)
            return Ok();

        return BadRequest(Resources.Get("INVALID_RESET_PASSWORD_TOKEN"));
    }

    [HttpPost("reset-password")]
    public ActionResult ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _userService.Value.ResetPassword(resetPasswordModel);

        return Ok();
    }

    [HttpGet("get-all")]
    [Authorize(Roles = "Admin, SysAdmin")]
    public ActionResult GetAll([FromQuery] PagingModel paging)
    {
        var users = _userService.Value.GetAllUsers(paging);

        return Ok(users);
    }

    [HttpGet("get-user-profile")]
    [Authorize]
    public ActionResult GetUserProfileById([FromQuery] int userId)
    {
        var user = _userService.Value.GetUserProfileById(userId);

        return Ok(user);
    }

    [HttpPost("update-user-info")]
    [Authorize]
    public ActionResult UpdateUserInfo([FromBody] UserProfileInfo userInfo)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _userService.Value.UpdateUserInfo(userInfo);

        return Ok();
    }

    [HttpPost("set-user-role")]
    [Authorize(Roles = "SysAdmin")]
    public ActionResult SetUserRole([FromBody] SetUserRoleModel model)
    {
        _userService.Value.SetUserRole(model.UserId, model.Role);

        return Ok();
    }

    [HttpPost("do-db-backup")]
    [Authorize(Roles = "SysAdmin")]
    public ActionResult DoBackup()
    {
        _userService.Value.DoDatabaseBackup();

        return Ok();
    }
}
