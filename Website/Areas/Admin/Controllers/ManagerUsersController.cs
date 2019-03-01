﻿using ApplicationCore.Services;
using Infrastructure.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Web.Mvc;
using Website.Configuaration;

namespace Website.Areas.Admin.Controllers
{
    public class ManagerUsersController : Controller
    {
        private readonly IApplicationUserService _userService;

        public ManagerUsersController(IApplicationUserService userService)
        {
            _userService = userService;
        }

        // GET: Admin/ManagerUsers
        public ActionResult Index()
        {
            var models = _userService.GetAll();
            return View(models);
        }

        public ActionResult Edit(string id)
        {
            ApplicationUser user = _userService.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser user)
        {
            _userService.Update(user);
            return RedirectToAction("Home");
        }

        [CustomRoleAuthorize(Roles = "Admin")]
        public ActionResult EditRole(string Id)
        {
            var model = _userService.Find(Id);

            IEnumerable<IdentityRole> roles = _userService.GetRolesByUserId(Id);

            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToRole(string userId, string roleId)
        {
            if (roleId != null && userId != null)
            {
                _userService.AddRoleToUser(userId, roleId);
            }

            return RedirectToAction("Index");
        }
    }
}