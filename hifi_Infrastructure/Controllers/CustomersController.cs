using hifi_Infrastructure.Models;
using hifi_Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hifi_Infrastructure.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.OrderByDescending(u => u.Name).ToListAsync();

            var usersWithRoles = new List<(User user, IList<string> roles)>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add((user, roles));
            }

            return View(usersWithRoles);
        }

        // GET: Customers/Details/6
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;
            return View(user);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    TempData["Success"] = $"Користувача {model.Email} створено успішно.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        // GET: Customers/Edit/6
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault() ?? "Customer";

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = currentRole
            };
            return View(model);
        }

        // POST: Customers/Edit/6
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, model.Role);

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (!passResult.Succeeded)
                {
                    foreach (var error in passResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            TempData["Success"] = "Дані користувача оновлено.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Customers/Delete/6
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;
            return View(user);
        }

        // POST: Customers/Delete/6
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();

                var currentUserId = _userManager.GetUserId(User);
                if (user.Id == currentUserId)
                {
                    TempData["Error"] = "Не можна видалити власний акаунт.";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Помилка при видаленні користувача.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = "Користувача видалено.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["Error"] = "Неможливо видалити покупця існує замовлення пов'язане з ними.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
