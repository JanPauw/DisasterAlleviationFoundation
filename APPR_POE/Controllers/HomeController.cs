using APPR_POE.Data;
using APPR_POE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace APPR_POE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Db_Context _context;

        public HomeController(ILogger<HomeController> logger, Db_Context context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var AllDisasters = await _context.Disasters.ToListAsync();
            var AllMoneyAllocations = await _context.MoneyAllocations.ToListAsync();
            var AllGoodsAllocations = await _context.GoodsAllocations.ToListAsync();

            ViewBag.ListDisasters = AllDisasters;
            ViewBag.ListMoneyAllocations = AllMoneyAllocations;
            ViewBag.ListGoodsAllocations = AllGoodsAllocations;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}