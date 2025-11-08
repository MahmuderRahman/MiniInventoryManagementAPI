using Microsoft.EntityFrameworkCore;
using MiniInventoryManagementAPI.Models;

namespace MiniInventoryManagementAPI.Context
{
    public class MiniInventoryDbContext:DbContext
    {
        public MiniInventoryDbContext(DbContextOptions<MiniInventoryDbContext> options): base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
