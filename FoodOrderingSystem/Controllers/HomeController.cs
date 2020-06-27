using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Dao;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FoodOrderingSystem.Dao.Entity;
using Microsoft.AspNetCore.Authorization;

namespace FoodOrderingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dbContext;

        public HomeController(DataContext dbContex)
        {
            _dbContext = dbContex;
        }

        [Authorize(Roles = "1")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/Home/Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Login(string name, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Name == name && x.Password == password);
            if (user == null) return Json(new { result = false });

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, name));
            identity.AddClaim(new Claim("UserId", user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.RoleId.ToString()));
            HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            return Json(new { result = true, role = user.RoleId });
        }


        //添加新用户
        [HttpPost]
        public IActionResult AddUser(string name, string password, string address, string phone)
        {
            //新建一个用户的新对象
            var user = new User()
            {
                Name = name,
                Password = password,
                Address = address,
                PhoneNum = phone,
                CreateTime = DateTime.Now,
                RoleId = (int)UserRole.User
            };
            _dbContext.Users.Add(user);

            return Json(new { result = _dbContext.SaveChanges() > 0});
        }

        public IActionResult Goods()
        {
            return View();
        }
    }
}
