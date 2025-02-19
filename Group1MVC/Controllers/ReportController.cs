using AutoMapper;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using BusinessServiceLayer.Services;
using Group1MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Specifications.NewsArticles;

namespace Group1MVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reporteService;
        private readonly FuNewsManagementContext _context;

        public ReportController(IReportService reporteService, FuNewsManagementContext context)
        {

            _reporteService = reporteService;
            _context = context;
        }
        public async Task<IActionResult> Index(NewsArticleSearchViewModel model)
        {
            if (ModelState.IsValid && model.StartDate.HasValue && model.EndDate.HasValue)
            {
                model.Results = await _reporteService.GetNewsArticlesByDateRangeAsync(model.StartDate.Value, model.EndDate.Value);
            }
            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var newsArticle = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticles.FindAsync(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryDescription", newsArticle.CategoryId);
            ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "Id", "Id", newsArticle.CreatedById);
            return View(newsArticle);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }


        public async Task<IActionResult> Index1(NewsArticleSearchByCreatedIdViewModel model)
        {
            if (ModelState.IsValid && model.CreatedById.HasValue)
            {
                model.Results = await _reporteService.GetNewsArticlesByCreatedByIdAsync(model.CreatedById.Value);
            }
            return View(model);
        }
    }
}
