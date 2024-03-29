﻿#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Text;
using Test.Areas.Identity.Data;
using Test.Data;
using Test.Data.Enum;
using Test.Data.Services;
using Test.Models;

namespace Test.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationController(INotificationService notificationService, UserManager<ApplicationUser> userManager)
        {
            _notificationService = notificationService;
            this._userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var myNotifications = _notificationService.GetAll().Where(n => n.UserId == user.Id).ToList();
            return View(myNotifications);
        }

        [Authorize]
        public async Task<IActionResult> Read(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var noti = _notificationService.GetById(id);
            if (noti != default)
            {
                if (noti.Status == NotificationStatus.unread)
                {
                    noti.Status = NotificationStatus.read;
                    _notificationService.Save();
                }
                return Redirect(noti.Link);
            }
            return View("NotFoundPage", "Error");
        }

        public IActionResult ReadAll()
        {
            var userId = _userManager.GetUserId(User);
            var userNotifications = _notificationService.GetAll().Where(n => n.UserId == userId && n.Status == NotificationStatus.unread);
            var userNotificationsId = userNotifications.Select(n => n.Id).ToList();
            foreach (var noti in userNotifications)
            {
                noti.Status = NotificationStatus.read;
                _notificationService.Save();
            }
            return Json(new { notificationsId = userNotificationsId });
        }

        public IActionResult CheckUnread()
        {
            var userId = _userManager.GetUserId(User);
            var checkUnread = _notificationService.CheckUnread(userId);
            return Json(new { unread = checkUnread });
		}
	}
}
