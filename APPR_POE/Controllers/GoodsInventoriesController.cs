using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APPR_POE.Data;
using APPR_POE.Models;

namespace APPR_POE.Controllers
{
    public class GoodsInventoriesController : Controller
    {
        private readonly Db_Context _context;

        public GoodsInventoriesController(Db_Context context)
        {
            _context = context;
        }

        // GET: GoodsInventories
        public async Task<IActionResult> Index()
        {
            //Total Money Allocated.
            double TotalAllocated = _context.MoneyStatements.Where(x => x.type == "allocate").Sum(x => x.amount);
            //Total Money used to buy goods.
            double TotalSpent = _context.MoneyStatements.Where(x => x.type == "acquired-goods").Sum(x => x.amount);
            //Money Total
            double TotalReceived = _context.MoneyStatements.Where(x => x.type == "donation").Sum(x => x.amount);
            ViewBag.AvailableMoney = TotalReceived - TotalAllocated - TotalSpent;

            //Goods Categories
            ViewBag.GoodsCategories = _context.GoodsInventories.Select(x => x.category).Distinct().ToList();

            return View(await _context.GoodsInventories.ToListAsync());
        }

        // GET: GoodsInventories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GoodsInventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string category, int quantity, double price, string email)
        {
            //Check for null/empty Category
            if (string.IsNullOrWhiteSpace(category))
            {
                TempData["error"] = "Invalid Category Chosen!";
                return RedirectToAction("Index");
            }

            //Check for Valid Category
            var gi = _context.GoodsInventories.Where(x => x.category == category).FirstOrDefault();
            if (gi == null)
            {
                TempData["error"] = "Invalid Category Chosen!";
                return RedirectToAction("Index");
            }

            //Check valid Quantity
            if (quantity < 1)
            {
                TempData["error"] = "Quantity cannot be lower than 0!";
                return RedirectToAction("Index");
            }

            //Check Valid Price
            if (price <= 0)
            {
                TempData["error"] = "Price cannot be R0.00 or lower!";
                return RedirectToAction("Index");
            }

            //Check for null/empty Email
            if (String.IsNullOrEmpty(email))
            {
                TempData["error"] = "This user cannot buy additional goods!";
                return RedirectToAction("Index");
            }

            //Check for valid User Email
            var user = _context.Users.Where(x => x.email == email).FirstOrDefault();
            if (user == null)
            {
                TempData["error"] = "This user cannot buy additional goods!";
                return RedirectToAction("Index");
            }

            //Set New GoodsInventory Quantity
            gi.quantity = gi.quantity + quantity;
            _context.GoodsInventories.Update(gi);

            //Add Record to MoneyStatements
            MoneyStatement ms = new MoneyStatement();
            ms.date = DateTime.Now;
            ms.type = "acquired-goods";
            ms.amount = (price * quantity);
            ms.donor = email;

            _context.MoneyStatements.Add(ms);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
