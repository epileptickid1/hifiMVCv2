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
    public class InternalpartsbrandsController : Controller
    {
        private readonly DbHifiContext _context;

        public InternalpartsbrandsController(DbHifiContext context)
        {
            _context = context;
        }

        // GET: Internalpartsbrands
        public async Task<IActionResult> Index()
        {
            return View(await _context.Internalpartsbrands.ToListAsync());
        }

        // GET: Internalpartsbrands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internalpartsbrand = await _context.Internalpartsbrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (internalpartsbrand == null)
            {
                return NotFound();
            }

            return View(internalpartsbrand);
        }

        // GET: Internalpartsbrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Internalpartsbrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Country,Id")] Internalpartsbrand internalpartsbrand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(internalpartsbrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(internalpartsbrand);
        }

        // GET: Internalpartsbrands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internalpartsbrand = await _context.Internalpartsbrands.FindAsync(id);
            if (internalpartsbrand == null)
            {
                return NotFound();
            }
            return View(internalpartsbrand);
        }

        // POST: Internalpartsbrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Country,Id")] Internalpartsbrand internalpartsbrand)
        {
            if (id != internalpartsbrand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(internalpartsbrand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternalpartsbrandExists(internalpartsbrand.Id))
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
            return View(internalpartsbrand);
        }

        // GET: Internalpartsbrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internalpartsbrand = await _context.Internalpartsbrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (internalpartsbrand == null)
            {
                return NotFound();
            }

            return View(internalpartsbrand);
        }

        // POST: Internalpartsbrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var internalpartsbrand = await _context.Internalpartsbrands.FindAsync(id);
                if (internalpartsbrand != null)
                {
                    _context.Internalpartsbrands.Remove(internalpartsbrand);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Неможливо видалити деталь, існують пов'язані навушники.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool InternalpartsbrandExists(int id)
        {
            return _context.Internalpartsbrands.Any(e => e.Id == id);
        }
    }
}
