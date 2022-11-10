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
            var SumOfGoodsDonations = await _context.GoodsDonations.SumAsync(x => x.quantity);
            double TotalReceived = await _context.MoneyStatements.Where(x => x.type == "donation").SumAsync(x => x.amount);

            List<Allocation> allocations = await GetAllocations();

            ViewBag.Allocations = allocations;
            ViewBag.ListDisasters = AllDisasters;
            ViewBag.SumOfGoodsDonations = SumOfGoodsDonations;
            ViewBag.TotalMoneyReceived = TotalReceived;

            return View();
        }

        public async Task<List<Allocation>> GetAllocations()
        {
            List<Allocation> toReturn = new List<Allocation>();

            var AllMoneyAllocations = await _context.MoneyAllocations.ToListAsync();
            var AllGoodsAllocations = await _context.GoodsAllocations.ToListAsync();

            foreach (var item in AllMoneyAllocations)
            {
                Allocation allocation = new Allocation();

                allocation.date = item.date;
                allocation.disaster = item.disaster_id;
                allocation.amount = item.amount;
                allocation.type = "money";

                toReturn.Add(allocation);
            }

            foreach (var item in AllGoodsAllocations)
            {
                Allocation allocation = new Allocation();

                allocation.date = item.date;
                allocation.disaster = item.disaster_id;
                allocation.amount = item.quantity;
                allocation.type = "good";

                toReturn.Add(allocation);
            }

            return toReturn;
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