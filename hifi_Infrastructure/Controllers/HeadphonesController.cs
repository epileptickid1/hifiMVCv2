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
            return View(await _context.Headphones.ToListAsync());
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

        // GET: Headphones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Headphones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Framematerial,Weight,Id")] Headphone headphone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(headphone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(headphone);
        }

        // GET: Headphones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headphone = await _context.Headphones.FindAsync(id);
            if (headphone == null)
            {
                return NotFound();
            }
            return View(headphone);
        }

        // POST: Headphones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Price,Framematerial,Weight,Id")] Headphone headphone)
        {
            if (id != headphone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(headphone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeadphoneExists(headphone.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
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
