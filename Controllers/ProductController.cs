using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PROG7311_PART_2.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PROG7311_PART_2.Controllers
{
    [Authorize(Roles = "Employee, Farmer")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Product list
        public async Task<IActionResult> Index(string farmerName, string productType, DateTime? from, DateTime? to)
        {
            var user = await _userManager.GetUserAsync(User);

            var products = _context.Products.Include(p => p.Farmer).AsQueryable();

            if (User.IsInRole("Farmer"))
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                if (farmer != null)
                    products = products.Where(p => p.FarmerId == farmer.Id);
            }

            if (!string.IsNullOrEmpty(farmerName))
                products = products.Where(p => p.Farmer.Name.Contains(farmerName));

            if (!string.IsNullOrEmpty(productType))
                products = products.Where(p => p.Category == productType);

            if (from.HasValue)
                products = products.Where(p => p.ProductionDate >= from.Value);

            if (to.HasValue)
                products = products.Where(p => p.ProductionDate <= to.Value);

            return View(await products.ToListAsync());
        }

        // GET: Add Product
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var user = await _userManager.GetUserAsync(User);
            List<Farmer> farmers;

            if (User.IsInRole("Employee"))
            {
                farmers = await _context.Farmers.ToListAsync();
            }
            else
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                farmers = new List<Farmer> { farmer };
            }

            var viewModel = new ProductViewModel
            {
                Product = new Product(),
                Farmers = farmers
            };

            return View(viewModel);
        }

        // POST: Add Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                if (User.IsInRole("Employee"))
                {
                    viewModel.Farmers = await _context.Farmers.ToListAsync();
                }
                else
                {
                    var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                    viewModel.Farmers = new List<Farmer> { farmer };
                }

                return View(viewModel);
            }

            if (!User.IsInRole("Employee"))
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);
                if (farmer != null)
                    viewModel.Product.FarmerId = farmer.Id;
            }

            _context.Products.Add(viewModel.Product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Edit Product
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);

            if (!User.IsInRole("Employee") && farmer != null && product.FarmerId != farmer.Id)
                return Forbid();

            var farmers = User.IsInRole("Employee")
                ? await _context.Farmers.ToListAsync()
                : new List<Farmer> { farmer };

            var viewModel = new ProductViewModel
            {
                Product = product,
                Farmers = farmers
            };

            return View(viewModel);
        }

        // POST: Edit Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == user.Id);

            if (!ModelState.IsValid)
            {
                viewModel.Farmers = User.IsInRole("Employee")
                    ? await _context.Farmers.ToListAsync()
                    : new List<Farmer> { farmer };

                return View(viewModel);
            }

            if (!User.IsInRole("Employee") && viewModel.Product.FarmerId != farmer?.Id)
                return Forbid();

            _context.Products.Update(viewModel.Product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
