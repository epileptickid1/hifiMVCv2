using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hifiDomain.Model;
using hifi_Infrastructure;

namespace hifi_Infrastructure.Controllers
{
    public class HeadphonesController : Controller
    {
        private readonly DbHifiContext _context;

        public HeadphonesController(DbHifiContext context)
        {
            _context = context;
        }

        // GET: Headphones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Headphones 
                .OrderBy(h => h.Id)
                .ToListAsync());
        }

        // GET: Headphones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headphone = await _context.Headphones
                .Include(h => h.Internalpartsbrands)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (headphone == null)
            {
                return NotFound();
            }

            return View(headphone);
        }
        // GET: Headphones/Create/5
        public IActionResult Create()
        {
            ViewData["Internalpartsbrands"] = new MultiSelectList(
                _context.Internalpartsbrands, "Id", "Name");
            return View();
        }

        // POST: Headphones/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,Price,Framematerial,Weight,Id")] Headphone headphone,
            int[] selectedBrands)
        {
            
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            
            if (ModelState.IsValid)
            {
                
                _context.Add(headphone);
                await _context.SaveChangesAsync();

                // Додаємо зв'язки з деталями
                if (selectedBrands != null)
                {
                    var brands = await _context.Internalpartsbrands
                        .Where(b => selectedBrands.Contains(b.Id))
                        .ToListAsync();
                    foreach (var brand in brands)
                        headphone.Internalpartsbrands.Add(brand);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Internalpartsbrands"] = new MultiSelectList(
                _context.Internalpartsbrands, "Id", "Name");
            return View(headphone);
        }

        // GET: Headphones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var headphone = await _context.Headphones
                .Include(h => h.Internalpartsbrands)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (headphone == null) return NotFound();

            var selectedIds = headphone.Internalpartsbrands.Select(b => b.Id);
            ViewData["Internalpartsbrands"] = new MultiSelectList(
                _context.Internalpartsbrands, "Id", "Name", selectedIds);

            return View(headphone);
        }

        // POST: Headphones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Name,Description,Price,Framematerial,Weight,Id")] Headphone headphone,
            int[] selectedBrands)
        {
            ModelState.Remove("Internalpartsbrands");
            
            if (id != headphone.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Завантажуємо з поточними зв'язками
                    var existing = await _context.Headphones
                        .Include(h => h.Internalpartsbrands)
                        .FirstAsync(h => h.Id == id);

                    // Оновлюємо поля
                    existing.Name = headphone.Name;
                    existing.Description = headphone.Description;
                    existing.Price = headphone.Price;
                    existing.Framematerial = headphone.Framematerial;
                    existing.Weight = headphone.Weight;

                    // Оновлюємо зв'язки
                    existing.Internalpartsbrands.Clear();
                    if (selectedBrands != null)
                    {
                        var brands = await _context.Internalpartsbrands
                            .Where(b => selectedBrands.Contains(b.Id))
                            .ToListAsync();
                        foreach (var brand in brands)
                            existing.Internalpartsbrands.Add(brand);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeadphoneExists(headphone.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Internalpartsbrands"] = new MultiSelectList(
                _context.Internalpartsbrands, "Id", "Name", selectedBrands);
            return View(headphone);
        }

        // GET: Headphones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headphone = await _context.Headphones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (headphone == null)
            {
                return NotFound();
            }

            return View(headphone);
        }

        // POST: Headphones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var headphone = await _context.Headphones.FindAsync(id);
                if (headphone != null)
                {
                    _context.Headphones.Remove(headphone);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Неможливо видалити навушник, існує пов'язані замовлення.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool HeadphoneExists(int id)
        {
            return _context.Headphones.Any(e => e.Id == id);
        }
    }
}
