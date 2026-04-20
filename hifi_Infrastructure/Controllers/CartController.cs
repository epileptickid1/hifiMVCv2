using Microsoft.AspNetCore.Mvc;
using hifiDomain.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace hifi_Infrastructure.Controllers
{
    public class CartController : Controller
    {
        private readonly DbHifiContext _context;
        private const string CartKey = "Cart";

        public CartController(DbHifiContext context)
        {
            _context = context;
        }

        
        private List<int> GetCart()
        {
            var json = HttpContext.Session.GetString(CartKey);
            return json == null ? new List<int>() : JsonSerializer.Deserialize<List<int>>(json)!;
        }

        
        private void SaveCart(List<int> cart)
        {
            HttpContext.Session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }

        
        public IActionResult AddToCart(int id)
        {
            var cart = GetCart();
            cart.Add(id);
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Index()
        {
            var cart = GetCart();
            var headphones = await _context.Headphones
                .Where(h => cart.Contains(h.Id))
                .ToListAsync();
            return View(headphones);
        }

        
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            cart.Remove(id);
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction("Index");

            ViewData["Cart"] = cart;
            ViewData["Customerid"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Customers, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int Customerid)
        {
            var cart = GetCart();

            var order = new Order
            {
                Orderdate = DateOnly.FromDateTime(DateTime.Now),
                Customerid = Customerid,
                Totalamount = await _context.Headphones
                    .Where(h => cart.Contains(h.Id))
                    .SumAsync(h => h.Price ?? 0)
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            
            var orderWithHeadphones = await _context.Orders
            .Include(o => o.Headphones)
            .FirstAsync(o => o.Id == order.Id);

            var headphonesToAdd = await _context.Headphones
                .Where(h => cart.Contains(h.Id))
                .ToListAsync();

            foreach (var headphone in headphonesToAdd)
            {
                orderWithHeadphones.Headphones.Add(headphone);
            }

            await _context.SaveChangesAsync();

            
            SaveCart(new List<int>());

            return RedirectToAction("Index", "Orders");
        }
    }
}