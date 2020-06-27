using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Dao.Entity
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNum { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
