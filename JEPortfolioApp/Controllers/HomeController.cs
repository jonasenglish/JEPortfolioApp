using JEPortfolioApp.Data;
using JEPortfolioApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JEPortfolioApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index([FromServices] IWebHostEnvironment env, string arg)
        {
            switch (arg)
            {
                case "LinkedIn":
                    LinkedIn();
                    break;
                case "GitHub":
                    GitHub();
                    break;
            }

            ViewData["enviroment"] = env;
            return View(await _context.Project.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public void GitHub()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/jonasenglish",
                UseShellExecute = true,
            });
        }
        
        public void LinkedIn()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.linkedin.com/in/jonasenglish/",
                UseShellExecute = true,
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}