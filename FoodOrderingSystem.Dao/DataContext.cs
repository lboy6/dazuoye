using FoodOrderingSystem.Dao.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Dao
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Food> Order_Foods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().Property(x => x.Name).HasMaxLength(200);
            modelBuilder.Entity<User>().Property(x => x.Password).HasMaxLength(200);
            modelBuilder.Entity<User>().Property(x => x.PhoneNum).HasMaxLength(200);
            modelBuilder.Entity<User>().Property(x => x.Address).HasMaxLength(500);

            modelBuilder.Entity<Food>().HasKey(x => x.Id);
            modelBuilder.Entity<Food>().Property(x => x.Name).HasMaxLength(200);
            modelBuilder.Entity<Food>().Property(x => x.Description).HasMaxLength(500);
            modelBuilder.Entity<Food>().Property(x => x.ImgUrl).HasMaxLength(500);

            modelBuilder.Entity<Order>().HasKey(x => x.Id);

            modelBuilder.Entity<Order_Food>().HasKey(x => x.Id);

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    RoleId = 1,
                    Name = "张三",
                    Password = "123",
                    PhoneNum = "18872818888",
                    Address = "湖北省武汉市洪山区高新大道12号 15栋8808",
                    CreateTime = DateTime.Now
                },
                new User()
                {
                    Id = 2,
                    RoleId = 2,
                    Name = "李四",
                    Password = "123",
                    PhoneNum = "18872818888",
                    Address = "湖北省武汉市洪山区高新大道12号 15栋8808",
                    CreateTime = DateTime.Now
                },
                new User()
                {
                    Id = 3,
                    RoleId = 2,
                    Name = "王五",
                    Password = "123",
                    PhoneNum = "18872818888",
                    Address = "湖北省武汉市洪山区高新大道12号 15栋8808",
                    CreateTime = DateTime.Now
                });

            modelBuilder.Entity<Food>().HasData(
               new Food()
               {
                   Id = 1,
                   Name = "青椒肉丝",
                   Description = "青椒肉丝",
                   Price = 12,
                   StockCount = 100,
                   ImgUrl = "~/img/1.jpg",
                   CreateTime = DateTime.Now
               },
                new Food()
                {
                    Id = 2,
                    Name = "手撕包菜",
                    Description = "手撕包菜",
                    Price = 12,
                    StockCount = 100,
                    ImgUrl = "~/img/2.jpg",
                    CreateTime = DateTime.Now
                },
                new Food()
                {
                    Id = 3,
                    Name = "青椒肉丝",
                    Description = "青椒肉丝",
                    Price = 12,
                    StockCount = 100,
                    ImgUrl = "~/img/1.jpg",
                    CreateTime = DateTime.Now
                });

            modelBuilder.Entity<Order>().HasData(
              new Order()
              {
                  Id = 1,
                  UserId = 1,
                  Price = 120,
                  CreateTime = DateTime.Now
              },
               new Order()
               {
                   Id = 2,
                   UserId = 3,
                   Price = 120,
                   CreateTime = DateTime.Now
               },
               new Order()
               {
                   Id = 3,
                   UserId = 3,
                   Price = 120,
                   CreateTime = DateTime.Now
               });

            modelBuilder.Entity<Order_Food>().HasData(
             new Order_Food()
             {
                 Id = 1,
                 OrderId = 1,
                 FoodId = 1,
             },
              new Order_Food()
              {
                  Id = 2,
                  OrderId = 1,
                  FoodId = 2,
              },
              new Order_Food()
              {
                  Id = 3,
                  OrderId = 2,
                  FoodId = 2
              });
        }
    }
}
