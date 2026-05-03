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


        private Dictionary<int, int> GetCart()
        {
            var json = HttpContext.Session.GetString(CartKey);
            return json == null ? new Dictionary<int, int>()
                : JsonSerializer.Deserialize<Dictionary<int, int>>(json)!;
        }


        private void SaveCart(Dictionary<int, int> cart)
        {
            HttpContext.Session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var cart = GetCart();
            if (cart.ContainsKey(id))
                cart[id] += quantity;
            else
                cart[id] = quantity;
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            cart.Remove(id);
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var cart = GetCart();
            if (quantity <= 0)
                cart.Remove(id);
            else
                cart[id] = quantity;
            SaveCart(cart);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var cart = GetCart();
            var headphones = await _context.Headphones
                .Where(h => cart.Keys.Contains(h.Id))
                .ToListAsync();

            ViewBag.Quantities = cart;
            return View(headphones);
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction("Index");

            ViewData["Customerid"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Customers, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(int Customerid)
        {
            var cart = GetCart();

            var headphones = await _context.Headphones
                .Where(h => cart.Keys.Contains(h.Id))
                .ToListAsync();

            int totalQuantity = cart.Values.Sum();

            decimal totalAmount = headphones.Sum(h => (h.Price ?? 0) * cart[h.Id]);

            var order = new Order
            {
                Orderdate = DateOnly.FromDateTime(DateTime.Now),
                Customerid = Customerid,
                Totalamount = totalAmount,
                Quantity = totalQuantity
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var orderWithHeadphones = await _context.Orders
                .Include(o => o.Headphones)
                .FirstAsync(o => o.Id == order.Id);

            foreach (var headphone in headphones)
            {
                orderWithHeadphones.Headphones.Add(headphone);
            }

            await _context.SaveChangesAsync();
            SaveCart(new Dictionary<int, int>());

            return RedirectToAction("Index", "Orders");
        }
    }
}