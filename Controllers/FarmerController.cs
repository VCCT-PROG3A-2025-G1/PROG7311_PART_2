using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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

        // GET: /Farmer/
        public async Task<IActionResult> Index()
        {
            var farmers = await _context.Farmers.ToListAsync();
            return View(farmers);
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);

            if (farmer == null) return NotFound();

            var products = await _context.Products
                .Where(p => p.FarmerId == farmer.Id)
                .ToListAsync();

            return View(products);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null) return NotFound();
            return View(farmer);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(Farmer farmer)
        {
            if (!ModelState.IsValid) return View(farmer);

            var existingFarmer = await _context.Farmers.FindAsync(farmer.Id);
            if (existingFarmer == null) return NotFound();

            // ✅ Update both Name and Location
            existingFarmer.Name = farmer.Name;
            existingFarmer.Location = farmer.Location;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}