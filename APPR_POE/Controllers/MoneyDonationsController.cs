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
    public class MoneyDonationsController : Controller
    {
        private readonly Db_Context _context;

        public MoneyDonationsController(Db_Context context)
        {
            _context = context;
        }

        // GET: MoneyDonations
        public async Task<IActionResult> Index()
        {
              return _context.MoneyDonations != null ? 
                          View(await _context.MoneyDonations.OrderByDescending(x => x.date).ToListAsync()) :
                          Problem("Entity set 'Db_Context.MoneyDonations'  is null.");
        }

        // GET: MoneyDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MoneyDonations == null)
            {
                return NotFound();
            }

            var moneyDonation = await _context.MoneyDonations
                .FirstOrDefaultAsync(m => m.id == id);
            if (moneyDonation == null)
            {
                return NotFound();
            }

            return View(moneyDonation);
        }

        // GET: MoneyDonations/Create
        public IActionResult Donate()
        {
            var test = Request.Headers["Referer"];
            ViewBag.Test = test;
            return View();
        }

        // POST: MoneyDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donate([Bind("id,date,amount,email,anonymous")] MoneyDonation moneyDonation)
        {
            if (ModelState.IsValid)
            {
                moneyDonation.date = DateTime.Now;
                _context.Add(moneyDonation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Users", new { id = moneyDonation.email });
            }

            TempData["success"] = "Donation added Successfully!";

            return View(moneyDonation);
        }

        private bool MoneyDonationExists(int id)
        {
          return (_context.MoneyDonations?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
