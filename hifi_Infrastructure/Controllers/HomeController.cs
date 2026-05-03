using hifi_Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace hifi_Infrastructure.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbHifiContext _context;

        public HomeController(DbHifiContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var headphones = _context.Headphones.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                headphones = headphones.Where(h => h.Name.ToLower().Contains(search.ToLower()));

            return View(await headphones.ToListAsync());
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
