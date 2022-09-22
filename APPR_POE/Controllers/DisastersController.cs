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

            if (start_date != null && end_date != null)
            {
                d.start_date = (DateTime)start_date;
                d.end_date = (DateTime)end_date;
            }

            if (location != null && description != null)
            {
                d.location = location;
                d.description = description;
            }

            if (aid_types != null)
            {
                d.aid_types = aid_types;
            }

            d.created_by = HttpContext.Session.GetString("email");

            _context.Add(d);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
