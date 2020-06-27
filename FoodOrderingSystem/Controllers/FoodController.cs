using System;
using System.IO;
using System.Linq;
using FoodOrderingSystem.Dao;
using FoodOrderingSystem.Dao.Entity;
using FoodOrderingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Controllers
{
    public class FoodController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FoodController(DataContext dbContex, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContex;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles = "1")]
        public IActionResult FoodList()
        {
            return View();
        }

        public IActionResult GetFoods(FilterModel filter)
        {
            var query = _dbContext.Foods.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(x => x.Name.Contains(filter.Keyword));
            }

            var foods = query.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize).ToList();
            return Json(new { Status = "Success", Data = foods, Total = query.Count() });
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult UploadImg()
        {
            try
            {
                var fileCount = Request.Form.Files.Count;
                if (fileCount == 0) return Json(new { Success = false });

                var file = Request.Form.Files[0];
                var folder = _hostingEnvironment.WebRootPath + "/img";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var physicalPath = Path.Combine(folder, Path.GetFileName(file.FileName));
                using (FileStream fs = System.IO.File.Create(physicalPath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                return Json(new { Success = true, fileName = $"/img/{Path.GetFileName(file.FileName)}"});
            }
            catch
            {
                return Json(new { Success = false });
            }
        }

        //向数据库里添加新产品
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult AddFood([FromBody]FoodDto food)
        {
            if (food.Id > 0)
            {
                var model = _dbContext.Foods.FirstOrDefault(x => x.Id == food.Id);
                model.Name = food.Name;
                model.Description = food.Description;
                model.Type = food.Type;
                model.ImgUrl = food.ImgUrl;
                model.Price = food.Price;
                model.StockCount = food.StockCount;
            }
            else
            {
                var foodModel = new Food()
                {
                    Name = food.Name,
                    Description = food.Description,
                    Type = food.Type,
                    ImgUrl = food.ImgUrl,
                    Price = food.Price,
                    StockCount = food.StockCount,
                    CreateTime = DateTime.Now
                };
                _dbContext.Foods.Add(foodModel);
            }

            return Json(new { Success = _dbContext.SaveChanges() > 0 });
        }
    }

    public class FoodDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
    }
}