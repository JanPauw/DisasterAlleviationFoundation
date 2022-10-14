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
    public class GoodsDonationsController : Controller
    {
        private readonly Db_Context _context;

        public GoodsDonationsController(Db_Context context)
        {
            _context = context;
        }

        // GET: GoodsDonations
        public async Task<IActionResult> Index()
        {
              return _context.GoodsDonations != null ? 
                          View(await _context.GoodsDonations.OrderByDescending(x => x.date).ToListAsync()) :
                          Problem("Entity set 'Db_Context.GoodsDonations'  is null.");
        }

        // GET: GoodsDonations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GoodsDonations == null)
            {
                return NotFound();
            }

            var goodsDonation = await _context.GoodsDonations
                .FirstOrDefaultAsync(m => m.id == id);
            if (goodsDonation == null)
            {
                return NotFound();
            }

            return View(goodsDonation);
        }

        // GET: GoodsDonations/Create
        public IActionResult Donate()
        {
            var outputList = _context.GoodsInventories.Select(x => x.category).Distinct().ToList();
            ViewData["Categories"] = outputList;            

            return View();
        }

        // POST: GoodsDonations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donate([Bind("id,date,quantity,category,description,email,anonymous")] GoodsDonation goodsDonation)
        {
            if (ModelState.IsValid)
            {
                goodsDonation.date = DateTime.Now;

                //Add Goods to Inventory
                GoodsInventory gi = _context.GoodsInventories.Where(x => x.category == goodsDonation.category).FirstOrDefault();
                if (gi == null)
                {
                    gi = new GoodsInventory();
                    gi.category = goodsDonation.category;
                    gi.quantity = goodsDonation.quantity;
                    _context.GoodsInventories.Add(gi);  
                }
                else
                {
                    gi.quantity = gi.quantity + goodsDonation.quantity;
                    _context.GoodsInventories.Update(gi);
                }

                _context.Add(goodsDonation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Users", new { id = goodsDonation.email });
            }

            TempData["success"] = "Donation added Successfully!";

            return View(goodsDonation);
        }

        private bool GoodsDonationExists(int id)
        {
          return (_context.GoodsDonations?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
