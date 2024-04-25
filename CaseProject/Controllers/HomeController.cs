using CaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CaseProject.Controllers
{
    public class HomeController : Controller
    {        
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ApplicationDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }


        public async Task<IActionResult> Index(int page = 1, int pageSize = 2)
        {
            int totalPosts = await _db.Blogs.CountAsync(b => b.Status == BlogPostStatus.Yayýnda);
            int totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            int skip = (page - 1) * pageSize;
            skip = Math.Max(0, skip);

            var publishedPosts = await _db.Blogs
                .Where(b => b.Status == BlogPostStatus.Yayýnda)
                .OrderByDescending(b => b.Date)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(publishedPosts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var blogPost = await _db.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(blogPost);
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
