using CaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaseProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        ILogger<BlogController> _logger;
        public BlogController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<BlogController> logger)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            // Pass the user's ID to the view
            ViewBag.UserId = currentUser.Id;

            // Fetch blog posts for the current user
            List<Blog> userPosts = await _db.Blogs
                .Where(b => b.AuthorId == currentUser.Id)
                .ToListAsync();
            return View(userPosts);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (string.IsNullOrWhiteSpace(blog.Title) || string.IsNullOrWhiteSpace(blog.Content) || string.IsNullOrWhiteSpace(blog.Category) || string.IsNullOrWhiteSpace(blog.Tags))
            {
                _logger.LogError("Creating failed due to missing input.");
            }
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(User);
                blog.Date = DateTime.Now;
                blog.bloggerName = currentUser.Name;
                blog.bloggerSurname = currentUser.SurName;
                blog.AuthorId = currentUser.Id;
                await _db.Blogs.AddAsync(blog);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Blog");
            }
            return View(blog);
        }

        public IActionResult Update(int? id)
        {
            Blog obj = new();
            obj = _db.Blogs.First(u => u.Id == id);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Blog blog)
        {
            if (string.IsNullOrWhiteSpace(blog.Title) || string.IsNullOrWhiteSpace(blog.Content) || string.IsNullOrWhiteSpace(blog.Category) || string.IsNullOrWhiteSpace(blog.Tags))
            {
                _logger.LogError("Updating failed due to missing or invalid input.");
            }

            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(User);
                blog.Date = DateTime.Now;
                _db.Blogs.Update(blog);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Blog");
            }
            return View(blog);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            Blog obj = new();
            obj = _db.Blogs.First(u => u.Id == id);
            _db.Blogs.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Blog");
        }
    }
}