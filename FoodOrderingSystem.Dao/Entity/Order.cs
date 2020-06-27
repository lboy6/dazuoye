using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Dao.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
