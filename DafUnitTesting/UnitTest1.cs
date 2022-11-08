using APPR_POE.Data;
using APPR_POE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DafUnitTesting
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

            //Add Test Data
            User u = new User();
            u.first_name = "Jan";
            u.last_name = "Pauw";
            u.phone = "0716108740";
            u.email = "pauwcoetzee@gmail.com";
            u.password = "TestPassword";
            u.role = "Admin";

            _context.Users.Add(u);
            _context.SaveChanges();
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