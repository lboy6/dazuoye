using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Dao.Entity
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
