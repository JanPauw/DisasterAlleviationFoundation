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
    public class DisastersController : Controller
    {
        private readonly Db_Context _context;

        public DisastersController(Db_Context context)
        {
            _context = context;
        }

        // GET: Disasters
        public async Task<IActionResult> Index()
        {
            return _context.Disasters != null ?
                        View(await _context.Disasters.OrderByDescending(x => x.start_date).ToListAsync()) :
                        Problem("Entity set 'Db_Context.Disasters'  is null.");
        }

        // GET: Disasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            List<GoodsInventory> outputList = _context.GoodsInventories.Distinct().ToList();
            ViewData["Inventory"] = outputList;

            if (id == null || _context.Disasters == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disasters
                .FirstOrDefaultAsync(m => m.id == id);
            if (disaster == null)
            {
                return NotFound();
            }

            //Total Money Allocated.
            double TotalAllocated = _context.MoneyStatements.Where(x => x.type == "allocate").Sum(x => x.amount);
            //Total money spent on acquiring goods
            double TotalSpent = _context.MoneyStatements.Where(x => x.type == "acquired-goods").Sum(x => x.amount);
            //Money Total
            double TotalReceived = _context.MoneyStatements.Where(x => x.type == "donation").Sum(x => x.amount);
            ViewData["MoneyLeft"] = TotalReceived - TotalAllocated - TotalSpent;
            ViewData["D_MoneyAllocated"] = _context.MoneyAllocations.Where(x => x.disaster_id == disaster.id).Sum(x => x.amount);
            ViewData["D_GoodsAllocated"] = _context.GoodsAllocations.Where(x => x.disaster_id == disaster.id).OrderByDescending(x => x.date).ToList();

            return View(disaster);
        }

        // GET: Disasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime? start_date, DateTime? end_date, string? location, string? description, string? aid_types)
        {
            Disaster d = new Disaster();

            #region Input Validation
            //Start Date
            try
            {
                d.start_date = (DateTime)start_date;
            }
            catch (Exception)
            {
                TempData["error"] = "Invalid Start Date Selected!";
                return RedirectToAction("Index");
            }

            //End Date
            try
            {
                d.end_date = (DateTime)end_date;
            }
            catch (Exception)
            {
                TempData["error"] = "Invalid End Date Selected!";
                return RedirectToAction("Index");
            }

            //Location
            if (String.IsNullOrEmpty(location))
            {
                TempData["error"] = "Invalid location entered!";
                return RedirectToAction("Index");
            }

            //Description
            if (String.IsNullOrEmpty(description))
            {
                TempData["error"] = "Invalid description entered!";
                return RedirectToAction("Index");
            }

            //Aid Types
            if (String.IsNullOrEmpty(aid_types))
            {
                TempData["error"] = "Invalid aid types added!";
                return RedirectToAction("Index");
            }
            #endregion

            d.location = location;
            d.description = description;
            d.aid_types = aid_types;
            d.created_by = HttpContext.Session.GetString("email");

            _context.Add(d);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateMoney(int disaster_id, double amount, string email)
        {
            var disaster = _context.Disasters.Find(disaster_id);
            if (disaster == null)
            {
                TempData["error"] = "Invalid Disaster Chosen for Allocation!";
                return RedirectToAction("Index");
            }

            //Money Total
            double TotalReceived = _context.MoneyStatements.Where(x => x.type == "donation").Sum(x => x.amount);
            double TotalAllocated = _context.MoneyStatements.Where(x => x.type == "allocate").Sum(x => x.amount);
            double TotalSpent = _context.MoneyStatements.Where(x => x.type == "acquired-goods").Sum(x => x.amount);
            var MoneyRemaining = TotalReceived - TotalAllocated - TotalSpent;

            if (amount > MoneyRemaining)
            {
                TempData["error"] = "Cannot allocate less than what remaining amount!";
                return RedirectToAction("Details", new { id = disaster.id });
            }

            var user = _context.Users.Find(email);
            if (user == null)
            {
                TempData["error"] = "Invalid user trying to allocate!";
                return RedirectToAction("Details", new { id = disaster.id });
            }

            //MoneyStatement Record Creation
            MoneyStatement ms = new MoneyStatement();
            ms.amount = amount;
            ms.date = DateTime.Now;
            ms.donor = email;
            ms.type = "allocate";

            //MoneyAllocation Record Creation
            MoneyAllocation ma = new MoneyAllocation();
            ma.amount = ms.amount;
            ma.date = ms.date;
            ma.disaster_id = disaster_id;

            _context.MoneyStatements.Add(ms);
            _context.MoneyAllocations.Add(ma);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = disaster.id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateGoods(int disaster_id, string category, int quantity)
        {
            var disaster = _context.Disasters.Find(disaster_id);
            if (disaster == null)
            {
                TempData["error"] = "Invalid Disaster Chosen for Allocation!";
                return RedirectToAction("Index");
            }

            if (String.IsNullOrWhiteSpace(category))
            {
                TempData["error"] = "Invalid category chosen!";
                return RedirectToAction("Details", new { id = disaster.id });
            }

            if (quantity < 1)
            {
                TempData["error"] = "Quantity cannot be lower than 0!";
                return RedirectToAction("Details", new { id = disaster.id });
            }

            GoodsInventory gi = _context.GoodsInventories.Where(x => x.category == category).FirstOrDefault();
            if (gi == null)
            {
                TempData["error"] = "Invalid category chosen!";
                return RedirectToAction("Details", new { id = disaster.id });
            }

            if (quantity > gi.quantity)
            {
                TempData["error"] = "Quantity cannot be more than available stock!";
                return RedirectToAction("Details", new { id = disaster.id });
            }

            //Update GoodsInventory with new quantity
            gi.quantity = gi.quantity - quantity;
            _context.GoodsInventories.Update(gi);

            //Add GoodsAllocation to DB
            GoodsAllocation ga = new GoodsAllocation();
            ga.date = DateTime.Now;
            ga.quantity = quantity;
            ga.category = category;
            ga.disaster_id = disaster_id;

            _context.GoodsAllocations.Add(ga);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = disaster_id });
        }

        // GET: Disasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Disasters == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disasters.FindAsync(id);
            if (disaster == null)
            {
                return NotFound();
            }
            return View(disaster);
        }

        // POST: Disasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,start_date,end_date,location,description,aid_types,created_by")] Disaster disaster)
        {
            if (id != disaster.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisasterExists(disaster.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = disaster.id });
            }
            return View(disaster);
        }

        // GET: Disasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Disasters == null)
            {
                return NotFound();
            }

            var disaster = await _context.Disasters
                .FirstOrDefaultAsync(m => m.id == id);
            if (disaster == null)
            {
                return NotFound();
            }

            return View(disaster);
        }

        // POST: Disasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Disasters == null)
            {
                return Problem("Entity set 'Db_Context.Disasters'  is null.");
            }
            var disaster = await _context.Disasters.FindAsync(id);
            if (disaster != null)
            {
                _context.Disasters.Remove(disaster);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisasterExists(int id)
        {
            return (_context.Disasters?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
