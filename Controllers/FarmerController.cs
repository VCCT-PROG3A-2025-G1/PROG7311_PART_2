using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311_PART_2.Models;
using Microsoft.AspNetCore.Authorization;

namespace PROG7311_PART_2.Controllers
{
    [Authorize(Roles = "Farmer,Employee")]
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Farmer/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var farmers = await _context.Farmers.ToListAsync();
            var viewModel = new Tuple<List<Farmer>, Farmer>(farmers, new Farmer());
            return View(viewModel);
        }

        // POST: Farmer/AddFarmer
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Farmer added successfully!";
                return RedirectToAction("Index");
            }

            // If invalid, reload the view with current list + validation errors
            var farmers = await _context.Farmers.ToListAsync();
            var viewModel = new Tuple<List<Farmer>, Farmer>(farmers, farmer);
            return View("Index", viewModel);
        }

        // GET: Farmer/Edit/{id}
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
                return NotFound();

            return View(farmer); // View: Views/Farmer/Edit.cshtml
        }

        // POST: Farmer/Edit
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(Farmer farmer)
        {
            if (!ModelState.IsValid)
            {
                return View(farmer);
            }

            var existingFarmer = await _context.Farmers.FindAsync(farmer.Id);
            if (existingFarmer == null)
            {
                return NotFound();
            }

            existingFarmer.Name = farmer.Name;
            existingFarmer.Location = farmer.Location;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Farmer updated successfully!";
            return RedirectToAction("Index");
        }

        // POST: Farmer/Delete/{id}
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Farmer deleted successfully!";
            }

            return RedirectToAction("Index");
        }

        // GET: Farmer/Dashboard (for Farmers only)
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);

            if (farmer == null)
                return NotFound();

            var products = await _context.Products
                .Where(p => p.FarmerId == farmer.Id)
                .ToListAsync();

            return View(products);
        }
    }
}

