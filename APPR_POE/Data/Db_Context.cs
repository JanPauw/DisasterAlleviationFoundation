using APPR_POE.Models;
using Microsoft.EntityFrameworkCore;

namespace APPR_POE.Data
{
    public class Db_Context : DbContext
    {
        public Db_Context(DbContextOptions<Db_Context> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<MoneyDonation> MoneyDonations { get; set; }
        public DbSet<GoodsDonation> GoodsDonations { get; set; }
        public DbSet<Disaster> Disasters { get; set; }
        public DbSet<MoneyStatement> MoneyStatements { get; set; }
        public DbSet<GoodsInventory> GoodsInventories { get; set; }
        public DbSet<MoneyAllocation> MoneyAllocations { get; set; }
        public DbSet<GoodsAllocation> GoodsAllocations { get; set; }
    }
}
