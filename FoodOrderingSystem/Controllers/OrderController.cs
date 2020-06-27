using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FoodOrderingSystem.Dao;
using FoodOrderingSystem.Dao.Entity;
using FoodOrderingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _dbContext;

        public OrderController(DataContext dbContex)
        {
            _dbContext = dbContex;
        }

        [Authorize(Roles = "1")]
        public IActionResult OrderList()
        {
            return View();
        }

        [Authorize(Roles = "1")]
        public IActionResult OrderDetail()
        {
            return View();
        }

        public IActionResult Test(string ok)
        {
            if (ok == "Ok")
                return Ok("好的");

            return BadRequest("BadRequest");
        }

        public IActionResult GetOrders(FilterModel filter)
        {
            var data = from a in _dbContext.Orders
                       join d in _dbContext.Users
                       on a.UserId equals d.Id
                       select new
                       {
                           Id = a.Id,
                           UserName = d.Name,
                           Address = d.Address,
                           a.CreateTime,
                           a.Status,
                           a.Price
                       };

            var orders = data.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize).ToList();
            return Json(new { Status = "Success", Data = orders, Total = data.Count() });
        }


        //用于保存该条订单记录的最新信息
        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult Save(int id, string status)
        {           
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == id);
            if (order != null)
            {
                order.Status = status;
                _dbContext.SaveChanges();
            }

            return Json(new { Status = "Success" });
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddOrder(decimal totalPrice, string foodIds)
        {
            //取出用户信息
            var userIdStr = User.Claims.SingleOrDefault(s => s.Type == "UserId").Value;
            int.TryParse(userIdStr, out int userId);

            //新建订单对象并且赋值
            var order = new Order()
            {
                UserId = userId,
                Price = totalPrice,
                Status = "已付款",
                CreateTime = DateTime.Now
            };
            //将新建的对象插入到数据库里
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            //新建order和food表的中间表的记录对象将其插入到数据库里
            var splitedIds = foodIds.Split(',');
            var addedIds = new List<Order_Food>();
            foreach (var id in splitedIds)
            {
                int.TryParse(id, out int foodId);
                var added = addedIds.FirstOrDefault(x => x.FoodId == foodId);
                if (added == null)
                {
                    var orderFood = new Order_Food()
                    {
                        OrderId = order.Id,
                        FoodId = foodId,
                        Nums = 1
                    };
                    _dbContext.Order_Foods.Add(orderFood);
                }
                else
                {
                    added.Nums++;
                }
            }

            _dbContext.SaveChanges();
            return Json(new { Status = "Success" });
        }
    }
}