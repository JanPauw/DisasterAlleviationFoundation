using APPR_POE.Controllers;
using APPR_POE.Data;
using APPR_POE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DafXUnitTesting
{
    public class UnitTest1 : IDisposable
    {
        private readonly Db_Context _context;

        public UnitTest1()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<Db_Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            _context = new Db_Context(options);
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetDisasters_ReturnsCorrectType()
        {
            var disasters = await _context.Disasters.ToListAsync();
            var model = Assert.IsAssignableFrom<IEnumerable<Disaster>>(disasters);
        }

        [Fact]
        public async Task GetGoodsAllocations_ReturnsCorrectType()
        {
            var goodsAllocations = await _context.GoodsAllocations.ToListAsync();
            var model = Assert.IsAssignableFrom<IEnumerable<GoodsAllocation>>(goodsAllocations);
        }

        [Fact]
        public async Task GetGoodsDonations_ReturnsCorrectType()
        {
            var goodsDonations = await _context.GoodsDonations.ToListAsync();
            var model = Assert.IsAssignableFrom<IEnumerable<GoodsDonation>>(goodsDonations);
        }

        [Fact]
        public async Task GetGoodsInventories_ReturnsCorrectType()
        {
            var goodsInventories = await _context.GoodsInventories.ToListAsync();
            var model = Assert.IsAssignableFrom<IEnumerable<GoodsInventory>>(goodsInventories);
        }

        [Fact]
        public async Task GetMoneyAllocations_ReturnsCorrectType()
        {
            var moneyAllocations = await _context.MoneyAllocations.ToListAsync();
            var model = Assert.IsAssignableFrom<IEnumerable<MoneyAllocation>>(moneyAllocations);
        }

        [Fact]
        public async Task GetMoneyDonations_ReturnsCorrectType()
        {
            var moneyDonations = await _context.MoneyDonations.ToListAsync();
            var model = Assert.IsAssignableFrom<IEnumerable<MoneyDonation>>(moneyDonations);
        }

        [Fact]
        public async Task GetMoneyStatements_ReturnsCorrectType()
        {
            var moneyStatements = await _context.MoneyStatements.ToListAsync();
            var model = Assert.IsAssignableFrom<IEnumerable<MoneyStatement>>(moneyStatements);
        }

        [Fact]
        public async Task GetUsers_ReturnsCorrectType()
        {
            var users = await _context.Users.ToListAsync();
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(users);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}