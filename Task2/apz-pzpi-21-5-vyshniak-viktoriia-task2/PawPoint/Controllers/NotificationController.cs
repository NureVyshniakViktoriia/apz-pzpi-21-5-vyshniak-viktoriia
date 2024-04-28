using BLL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Infrastructure.Models;

namespace WebAPI.Controllers;
[Area("notification")]
[Route("api/[area]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("get-notifications-by-user-id")]
    public ActionResult GetAllHealthRecords([FromQuery] int userId)
    {
        var notifications = _notificationService.GetAllByUserId(userId);

        return Ok(notifications);
    }

    [HttpGet("get-notification-by-id")]
    public ActionResult GetById([FromQuery] Guid notificationId)
    {
        var notification = _notificationService.GetById(notificationId);

        return Ok(notification);
    }

    [HttpPost("notify-admin")]
    [AllowAnonymous]
    public ActionResult NotifyAdmin([FromBody] NotifyAdminModel notifyAdminModel)
    {
        _notificationService.Create(notifyAdminModel.UserId, notifyAdminModel.PetRFID);

        return Ok();
    }
}
