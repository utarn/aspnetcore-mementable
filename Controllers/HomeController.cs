using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using aspnetcore_mementable.Models;
using aspnetcore_mementable.Data;
using aspnetcore_mementable.MementoExtension.Services;
using Microsoft.EntityFrameworkCore;
using aspnetcore_mementable.MementoExtension.Interfaces;

namespace aspnetcore_mementable.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly StateDbContext _stateContext;
        private readonly StateManager<BlogPost> _stateManager;
        public HomeController(ILogger<HomeController> logger,
                              ApplicationDbContext context,
                              StateManager<BlogPost> stateManager,
                              StateDbContext stateContext)
        {
            _logger = logger;
            _context = context;
            _stateManager = stateManager;
            _stateContext = stateContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewBlog()
        {
            var allBlogs = await _context.BlogPosts.ToListAsync();
            return Json(allBlogs);
        }

        public async Task<IActionResult> ViewState()
        {
            var allStates = await _stateContext.States.ToListAsync();
            return Json(allStates);
        }
        public async Task<IActionResult> Push()
        {
            if (!_context.BlogPosts.Any())
            {
                var testPost = new BlogPost
                {
                    Content = "this is testing",
                    LastUpdated = DateTime.Now
                };
                _context.Add(testPost);
                await _context.SaveChangesAsync();
                await _stateManager.PushMemento(testPost);
            }
            else
            {
                var testPost = await _context.BlogPosts.FirstOrDefaultAsync();
                testPost.LastUpdated = DateTime.Now;
                await _context.SaveChangesAsync();
                await _stateManager.PushMemento(testPost);
            }
            return RedirectToAction("ViewState");
        }
        public async Task<IActionResult> Undo()
        {
            var lastBlogpost = await _stateManager.Undo();
            _context.Attach(lastBlogpost);
            _context.Entry(lastBlogpost).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Json(lastBlogpost);
        }
        public async Task<IActionResult> Redo()
        {
            var nextBlogpost = await _stateManager.Redo();
            _context.Attach(nextBlogpost);
            _context.Entry(nextBlogpost).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Json(nextBlogpost);
        }
    }
}
