using APPR_POE.Controllers;
using APPR_POE.Data;
using APPR_POE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DafMSUnitTesting
{
    [TestClass]
    public class UnitTest1
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

            PopulateDB();
        }

        [TestMethod]
        public void Login_Pass()
        {
            //Admin
            string email1 = "pauwcoetzee@gmail.com";
            string pass1 = "Stevieru1es!";
            
            //Denied
            string email2 = "henco@gmail.com";
            string pass2 = "12345678";

            //Member
            string email3 = "oliverh@gmail.com";
            string pass3 = "Password";

            //Pending
            string email4 = "jared@vc.co.za";
            string pass4 = "Password";

            //Context to access method
            UsersController usersController = new UsersController(_context);

            bool user1Login = usersController.ValidLogin(email1, pass1);
            bool user2Login = usersController.ValidLogin(email2, pass2);
            bool user3Login = usersController.ValidLogin(email3, pass3);
            bool user4Login = usersController.ValidLogin(email4, pass4);

            Assert.AreEqual(true, user1Login);
            Assert.AreEqual(false, user2Login);
            Assert.AreEqual(true, user3Login);
            Assert.AreEqual(false, user4Login);
        }

        [TestMethod]
        public void Login_Fail()
        {
            //Admin
            string email1 = "pauwcoetzee@gmail.com";
            string pass1 = "Stevirules!";

            //Denied
            string email2 = "henco@gmail.com";
            string pass2 = "123456789";

            //Member
            string email3 = "oliverh@gmail.com";
            string pass3 = "Passwords";

            //Pending
            string email4 = "jared@vc.co.za";
            string pass4 = "Password1";

            //Context to access method
            UsersController usersController = new UsersController(_context);

            bool user1Login = usersController.ValidLogin(email1, pass1);
            bool user2Login = usersController.ValidLogin(email2, pass2);
            bool user3Login = usersController.ValidLogin(email3, pass3);
            bool user4Login = usersController.ValidLogin(email4, pass4);

            Assert.AreEqual(false, user1Login);
            Assert.AreEqual(false, user2Login);
            Assert.AreEqual(false, user3Login);
            Assert.AreEqual(false, user4Login);
        }

        public void PopulateDB()
        {
            //Populate Users
            #region User 1 - pauwcoetzee@gmail.com
            User user1 = new User();
            user1.email = "pauwcoetzee@gmail.com";
            user1.password = "Vqi7Z76sfn7XfZLMAVINPQ==";
            user1.first_name = "Jan";
            user1.last_name = "Pauw";
            user1.phone = "0716108740";
            user1.role = "admin";

            _context.Users.Add(user1);
            #endregion

            #region User 2 - henco@gmail.com
            User user2 = new User();
            user2.email = "henco@gmail.com";
            user2.password = "KXBSHe+CyeY523o168gXRw==";
            user2.first_name = "Henco";
            user2.last_name = "Barkhuizen";
            user2.phone = "0235689451";
            user2.role = "denied";

            _context.Users.Add(user2);
            #endregion

            #region User 3 - oliverh@gmail.com
            User user3 = new User();
            user3.email = "oliverh@gmail.com";
            user3.password = "EqGbtIAOUMuxK5EsC/j/nQ==";
            user3.first_name = "Oliver";
            user3.last_name = "Honiball";
            user3.phone = "0812348764";
            user3.role = "member";

            _context.Users.Add(user3);
            #endregion

            #region User 4 - jared@vc.co.za
            User user4 = new User();
            user4.email = "jared@vc.co.za";
            user4.password = "EqGbtIAOUMuxK5EsC/j/nQ==";
            user4.first_name = "Jared";
            user4.last_name = "Dicker";
            user4.phone = "0824630731";
            user4.role = "pending";

            _context.Users.Add(user4);
            #endregion

            _context.SaveChanges();
        }
    }
}