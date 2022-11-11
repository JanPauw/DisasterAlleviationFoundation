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

            Users_PopulateDB();
        }

        #region User Testing
        [TestMethod]
        public void User_Login_Pass()
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
        public void User_Login_Fail()
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

        [TestMethod]
        public void User_ValidEmail_Pass()
        {
            UsersController usersController = new UsersController(_context);

            string validEmail = "pauwcoetzee@gmail.com";

            bool IsValidEmail = usersController.IsValidEmail(validEmail);

            Assert.AreEqual(true, IsValidEmail);
        }

        [TestMethod]
        public void User_ValidEmail_Fail()
        {
            UsersController usersController = new UsersController(_context);

            string validEmail = "pauwcoetzee";

            bool IsValidEmail = usersController.IsValidEmail(validEmail);

            Assert.AreEqual(false, IsValidEmail);
        }

        [TestMethod]
        public void User_Registered_Pass()
        {
            #region User 1 Testing
            User user1 = new User();
            user1.first_name = "Edward";
            user1.last_name = "Tierney";
            user1.phone = "9782078719";
            user1.email = "claria_swif4@yahoo.com";
            user1.password = "Vqi7Z76sfn7XfZLMAVINPQ==";
            user1.role = "pending";
            #endregion

            #region User 2 Testing
            User user2 = new User();
            user2.first_name = "Wade";
            user2.last_name = "Williams";
            user2.phone = "7023229958";
            user2.email = "lillie2017@gmail.com";
            user2.password = "Vqi7Z76sfn7XfZLMAVINPQ==";
            user2.role = "pending";
            #endregion

            #region User 3 Testing
            User user3 = new User();
            user3.first_name = "Jennifer";
            user3.last_name = "Gonzales";
            user3.phone = "5125906216";
            user3.email = "kurtis_balistre@yahoo.com";
            user3.password = "Password3";
            user3.role = "pending";
            #endregion

            _context.Users.Add(user1);
            _context.Users.Add(user2);
            _context.Users.Add(user3);

            _context.SaveChanges();

            Assert.IsNotNull(_context.Users.Find(user1.email));
            Assert.IsNotNull(_context.Users.Find(user2.email));
            Assert.IsNotNull(_context.Users.Find(user3.email));
        }

        [TestMethod]
        public void User_ValidRegistration_Pass()
        {
            UsersController usersController = new UsersController(_context);

            #region User 1 Testing
            string first_name1 = "Edward";
            string last_name1 = "Tierney";
            string phone1 = "9782078719";
            string email1 = "claria_swif4@yahoo.com";
            string password1 = "Password1";
            string confirm1 = "Password1";
            #endregion

            #region User 2 Testing
            string first_name2 = "Wade";
            string last_name2 = "Williams";
            string phone2 = "7023229958";
            string email2 = "lillie2017@gmail.com";
            string password2 = "Password2";
            string confirm2 = "Password2";
            #endregion

            #region User 3 Testing
            string first_name3 = "Jennifer";
            string last_name3 = "Gonzales";
            string phone3 = "5125906216";
            string email3 = "kurtis_balistre@yahoo.com";
            string password3 = "Password3";
            string confirm3 = "Password3";
            #endregion

            bool ValidRegistration1 = usersController.ValidRegistration(first_name1, last_name1, phone1, email1, password1, confirm1);
            bool ValidRegistration2 = usersController.ValidRegistration(first_name2, last_name2, phone2, email2, password2, confirm2);
            bool ValidRegistration3 = usersController.ValidRegistration(first_name3, last_name3, phone3, email3, password3, confirm3);

            Assert.AreEqual(true, ValidRegistration1);
            Assert.AreEqual(true, ValidRegistration2);
            Assert.AreEqual(true, ValidRegistration3);
        }

        [TestMethod]
        public void User_ValidRegistration_Fail()
        {
            UsersController usersController = new UsersController(_context);

            #region User 1 Testing (Invalid Phone Number)
            string first_name1 = "Edward";
            string last_name1 = "Tierney";
            string phone1 = "978207871A";
            string email1 = "claria_swif4@yahoo.com";
            string password1 = "Password1";
            string confirm1 = "Password1";
            #endregion

            #region User 2 Testing (Invalid Email Address)
            string first_name2 = "Wade";
            string last_name2 = "Williams";
            string phone2 = "7023229958";
            string email2 = "lillie2017";
            string password2 = "Password2";
            string confirm2 = "Password2";
            #endregion

            #region User 3 Testing (Passwords do not match)
            string first_name3 = "Jennifer";
            string last_name3 = "Gonzales";
            string phone3 = "5125906216";
            string email3 = "kurtis_balistre@yahoo.com";
            string password3 = "Password3";
            string confirm3 = "Passwords";
            #endregion

            bool ValidRegistration1 = usersController.ValidRegistration(first_name1, last_name1, phone1, email1, password1, confirm1);
            bool ValidRegistration2 = usersController.ValidRegistration(first_name2, last_name2, phone2, email2, password2, confirm2);
            bool ValidRegistration3 = usersController.ValidRegistration(first_name3, last_name3, phone3, email3, password3, confirm3);

            Assert.AreEqual(false, ValidRegistration1);
            Assert.AreEqual(false, ValidRegistration2);
            Assert.AreEqual(false, ValidRegistration3);
        }

        public void Users_PopulateDB()
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
        #endregion


    }
}