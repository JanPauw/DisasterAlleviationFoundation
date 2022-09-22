using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APPR_POE.Data;
using APPR_POE.Models;
using CargoHubWeb.Data;
using System.Configuration;
using System.Net.Mail;

namespace APPR_POE.Controllers
{
    public class UsersController : Controller
    {
        private readonly Db_Context _context;

        public UsersController(Db_Context context)
        {
            _context = context;
        }

        #region Login Functionaility
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string? email, string? password)
        {
            //Encryption Object for password
            Encrypt enc = new Encrypt();

            //Check if inputs are not empty
            if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password))
            {
                TempData["error"] = "Fields cannot be empty!";
                return View();
            }

            //encrypt input password for checking
            string encPassword = enc.EncryptString(password);
            password = encPassword;

            //find user with the email
            var objUser = _context.Users.Find(email);

            //check if user is found
            if (objUser == null)
            {
                TempData["error"] = "Email is not registered! Please sign up.";
                return View();
            }

            //check if email matches
            if (objUser.email != email)
            {
                TempData["error"] = "Invalid Login Credentials!";
                return View();
            }

            //check if password matches
            if (objUser.password != password)
            {
                TempData["error"] = "Invalid Login Credentials!";
                return View();
            }

            //check if user is still pending
            if (objUser.role == "pending")
            {
                TempData["error"] = "Your account is still pending authorization!";
                return View();
            }

            //check if user is denied access
            if (objUser.role == "denied")
            {
                TempData["error"] = "Your account has been denied access!";
                return View();
            }

            HttpContext.Session.SetString("email", objUser.email);
            HttpContext.Session.SetString("password", objUser.password);
            HttpContext.Session.SetString("first_name", objUser.first_name);
            HttpContext.Session.SetString("last_name", objUser.last_name);
            HttpContext.Session.SetString("phone", objUser.phone);
            HttpContext.Session.SetString("role", objUser.role);
            HttpContext.Session.SetString("logged_in", "true");

            TempData["success"] = "You are now logged in!";

            return RedirectToAction("Details", new { id = objUser.email });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["success"] = "You are now logged out!";
            return RedirectToAction("Login");
        }
        #endregion

        #region Sign Up Functionality
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(string? first_name, string? last_name, string? phone, string? email, string? password, string? confirm)
        {
            //Encryption Object for password
            Encrypt enc = new Encrypt();

            #region Check For Empty Fields
            if (String.IsNullOrWhiteSpace(first_name) || String.IsNullOrWhiteSpace(last_name))
            {
                TempData["error"] = "Fields cannot be empty!";
                return View();
            }

            if (String.IsNullOrWhiteSpace(phone) || String.IsNullOrWhiteSpace(email))
            {
                TempData["error"] = "Fields cannot be empty!";
                return View();
            }

            if (String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(confirm))
            {
                TempData["error"] = "Fields cannot be empty!";
                return View();
            }
            #endregion

            #region Check phone number
            if (phone.Length != 10)
            {
                TempData["error"] = "Invalid Phone Number!";
                return View();
            }

            try
            {
                int test_phone = int.Parse(phone);
            }
            catch (Exception)
            {
                TempData["error"] = "Phone Number has letters!";
                return View();
            }
            #endregion

            #region Email Validation
            if (IsValid(email) == false)
            {
                TempData["error"] = "Invalid Email!";
                return View();
            }
            #endregion

            #region Check Passwords
            //Password matches Confirm
            if (password != confirm)
            {
                TempData["error"] = "Passwords do not match!";
                return View();
            }

            //Password length is 8 or more
            if (password.Length < 8)
            {
                TempData["error"] = "Password needs to be 8+ characters!";
                return View();
            }

            //Encrypt Passwords
            string encPassword = enc.EncryptString(password);
            #endregion

            #region Write new User to Database
            User newUser = new User();

            newUser.first_name = first_name;
            newUser.last_name = last_name;
            newUser.email = email;
            newUser.phone = phone;
            newUser.password = encPassword;
            newUser.role = "pending";

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            #endregion

            TempData["success"] = "Successful registration!";

            return RedirectToAction("Login");
        }

        //Email Validation
        private static bool IsValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
        #endregion

        #region Admin Features
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(string? email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction(nameof(Index));
            }

            var objUser = _context.Users.Find(email);

            if (objUser == null)
            {
                return RedirectToAction(nameof(Index));
            }

            objUser.role = "member";

            _context.Update(objUser);
            await _context.SaveChangesAsync();

            TempData["success"] = "Approved: " + objUser.first_name + " " + objUser.last_name;

            return Redirect(Request.Headers["Referer"]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(string? email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction(nameof(Index));
            }

            var objUser = _context.Users.Find(email);

            if (objUser == null)
            {
                return RedirectToAction(nameof(Index));
            }

            objUser.role = "denied";

            _context.Update(objUser);
            await _context.SaveChangesAsync();

            TempData["success"] = "Denied: " + objUser.first_name + " " + objUser.last_name;

            return Redirect(Request.Headers["Referer"]);
        }
        #endregion

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.Where(x => x.role == "pending").ToListAsync()) :
                        Problem("Entity set 'Db_Context.Users'  is null.");
        }

        public async Task<IActionResult> Approved()
        {
            return _context.Users != null ?
                        View(await _context.Users.Where(x => x.role == "member").ToListAsync()) :
                        Problem("Entity set 'Db_Context.Users'  is null.");
        }

        public async Task<IActionResult> Denied()
        {
            return _context.Users != null ?
                        View(await _context.Users.Where(x => x.role == "denied").ToListAsync()) :
                        Problem("Entity set 'Db_Context.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.email == id);
            if (user == null)
            {
                return NotFound();
            }

            #region Lists of User's donations
            var listGoods = _context.GoodsDonations.Where(x => x.email == user.email && x.anonymous == false).OrderByDescending(x => x.date).ToList();
            var listMoney = _context.MoneyDonations.Where(x => x.email == user.email && x.anonymous == false).OrderByDescending(x => x.date).ToList();
            ViewData["GoodsDonations"] = listGoods;
            ViewData["MoneyDonations"] = listMoney;
            #endregion

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? first_name, string? last_name, string? phone, string? email)
        {
            //Encryption Object for password
            Encrypt enc = new Encrypt();

            #region Check For Empty Fields
            if (String.IsNullOrWhiteSpace(first_name) || String.IsNullOrWhiteSpace(last_name))
            {
                TempData["ErrorMessage"] = "Fields cannot be empty!";
                return View();
            }

            if (String.IsNullOrWhiteSpace(phone) || String.IsNullOrWhiteSpace(email))
            {
                TempData["ErrorMessage"] = "Fields cannot be empty!";
                return View();
            }
            #endregion

            #region Check phone number
            if (phone.Length != 10)
            {
                TempData["ErrorMessage"] = "Invalid Phone Number!";
                return View();
            }

            try
            {
                int test_phone = int.Parse(phone);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Phone Number has letters!";
                return View();
            }
            #endregion

            #region Write new details to Database
            var objUser = _context.Users.Find(email);

            objUser.first_name = first_name;
            objUser.last_name = last_name;
            objUser.phone = phone;

            _context.Users.Update(objUser);
            await _context.SaveChangesAsync();
            #endregion

            TempData["success"] = "Details Updated Successfully!";

            return RedirectToAction("Details", new { id = email });
        }
    }
}
