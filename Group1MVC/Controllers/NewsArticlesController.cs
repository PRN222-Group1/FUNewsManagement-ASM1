﻿using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Group1MVC.Helpers;
using Group1MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Specifications.NewsArticles;

namespace Group1MVC.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly FuNewsManagementContext _context;
        private readonly INewsArticleService _newsArticleService;
        private readonly IAccountService _accountService;

        public NewsArticlesController(FuNewsManagementContext context, INewsArticleService newsArticleService, IAccountService accountService)
        {
            _context = context;
            _newsArticleService = newsArticleService;
            _accountService = accountService;
        }

        // GET: NewsArticles
        public async Task<IActionResult> Index(NewsArticleSpecParams specParams)
        {
            var newsArticles = await _newsArticleService.GetNewsArticlesAsync(specParams);

            var count = await _newsArticleService.CountNewsArticlesAsync(specParams);

            var paginatedResult = new Pagination<NewsArticleDTO>(specParams.PageNumber, specParams.PageSize, count, newsArticles);

            var categories = await _newsArticleService.GetAllCategories();

            // Set current filter and sort values for use in the view
            ViewData["CurrentFilter"] = specParams.Search;
            ViewData["CurrentSort"] = specParams.Sort;
            ViewData["CurrentPageNumber"] = specParams.PageNumber;
            ViewData["CurrentPageSize"] = specParams.PageSize;
            ViewData["CurrentPageCount"] = Convert.ToInt32(Math.Ceiling((decimal)count / specParams.PageSize));

            ViewBag.Categories= categories;
            ViewData["SelectedCategory"] = specParams.CategoryId;

            return View(paginatedResult);
        }

        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);

            if (newsArticle == null) return NotFound();

            return View(newsArticle);
        }

        // GET: NewsArticles/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _newsArticleService.GetAllCategories();
            var tags = await _newsArticleService.GetAllTags();

            ViewBag.Categories = categories;
            ViewBag.Tags = tags;

            return View();
        }

        // POST: NewsArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsTitle,Headline,NewsContent,NewsSource,CategoryId,NewsStatus,TagIds")] NewsArticleToAddDTO newsArticle)
        {
            var result = false;

            // Set created and modified date to current time
            newsArticle.CreatedDate = DateTime.UtcNow;
            newsArticle.ModifiedDate = DateTime.UtcNow;

            // Get currently logged in user
            var email = User.Claims.FirstOrDefault().Value;
            var currentUser = await _accountService.GetUserByEmailAsync(email);

            // Set current user to the current logged in staff
            newsArticle.CreatedById = currentUser.Id;
            newsArticle.UpdatedById = currentUser.Id;

            if (ModelState.IsValid)
            {
                result = await _newsArticleService.CreateNewsArticle(newsArticle);
            }

            if (result) return RedirectToAction("Index", "NewsArticles");

            return View(newsArticle);
        }

        // GET: NewsArticles/Edit/5
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

        // POST: NewsArticles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsTitle,Headline,CreatedDate,NewsContent,NewsSource,CategoryId,NewsStatus,CreatedById,UpdatedById,ModifiedDate,Id")] NewsArticle newsArticle)
        {
            if (id != newsArticle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsArticleExists(newsArticle.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryDescription", newsArticle.CategoryId);
            ViewData["CreatedById"] = new SelectList(_context.SystemAccounts, "Id", "Id", newsArticle.CreatedById);
            return View(newsArticle);
        }

        // GET: NewsArticles/Delete/5
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

        // POST: NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsArticle = await _context.NewsArticles.FindAsync(id);
            if (newsArticle != null)
            {
                _context.NewsArticles.Remove(newsArticle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsArticleExists(int id)
        {
            return _context.NewsArticles.Any(e => e.Id == id);
        }
    }
}
