#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JEPortfolioApp.Data;
using JEPortfolioApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JEPortfolioApp.Controllers
{
    public class ProjectsController : Controller
    {
        public static ProjectsController Instance;

        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            Instance = this;
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
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

        #region Create

        // GET: Projects/Create
        [Authorize(Policy = "OwnerOnly")]
        public IActionResult Create([FromServices] IWebHostEnvironment env)
        {
            ViewData["enviroment"] = env.WebRootPath;

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,Description,ImagePath,FilePath,Month,Year")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Id = Guid.NewGuid();
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        #endregion

        #region Edit

        // GET: Projects/Edit/5
        [Authorize(Policy = "OwnerOnly")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "OwnerOnly")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ProjectName,Description,ImagePath,FilePath,Month,Year")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            return View(project);
        }

        #endregion

        #region Delete

        // GET: Projects/Delete/5
        [Authorize(Policy = "OwnerOnly")]
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: Projects/Delete/5
        [Authorize(Policy = "OwnerOnly")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Images

        // GET: Projects/SelectImage
        public IActionResult SelectImage(string name, string file, [FromServices] IWebHostEnvironment env)
        {
            ViewData["selectedImage"] = name;
            ViewData["fileLocation"] = file;
            ViewData["enviroment"] = env.WebRootPath;

            return View("Create");
        }

        [Authorize(Policy = "OwnerOnly")]
        public IActionResult ShowFieldsImage(IFormFile pic, [FromServices] IWebHostEnvironment env)
        {
            if (pic != null)
            {
                var fileName = Path.Combine(env.WebRootPath, $"images\\{Path.GetFileName(pic.FileName)}");

                using (var fs = new FileStream(fileName, FileMode.Create))
                    pic.CopyTo(fs);

                ViewData["picLocation"] = $"/images/{Path.GetFileName(pic.FileName)}";
            }

            return View();
        }

        #endregion

        #region File Management

        [Authorize(Policy = "OwnerOnly")]
        public IActionResult UploadFile(IFormFile file, [FromServices] IWebHostEnvironment env)
        {
            if (file != null)
            {
                var fileName = Path.Combine(env.WebRootPath, $"files\\{Path.GetFileName(file.FileName)}");

                using (var fs = new FileStream(fileName, FileMode.Create))
                    file.CopyTo(fs);

                ViewData["fileLocation"] = $"/files/{Path.GetFileName(file.FileName)}";
            }

            ViewData["enviroment"] = env.WebRootPath;

            return View("Create");
        }

        [HttpGet("download")]
        public IActionResult DownloadFile([FromQuery] string link, [FromServices] IWebHostEnvironment env)
        {
            if (link.Contains("https://"))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = link,
                    UseShellExecute = true,
                });
                return View("Index", _context.Project.ToList());
            }

            var net = new System.Net.WebClient();
            var data = net.DownloadData(String.Concat(env.WebRootPath, link));
            var content = new MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(link);
            return File(content, contentType, fileName);
        }

        #endregion

        private bool ProjectExists(Guid id)
        {
            return _context.Project.Any(e => e.Id == id);
        }
    }
}
