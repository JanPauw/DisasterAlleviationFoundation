using APPR_POE.Data;
using APPR_POE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DafMSUnitTesting
{
    [TestClass]
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}