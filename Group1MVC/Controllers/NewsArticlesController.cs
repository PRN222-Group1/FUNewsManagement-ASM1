using BusinessServiceLayer.DTOs;
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
            ViewData["Action"] = "Create";

            return View();
        }

        // POST: NewsArticles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsTitle,Headline,NewsContent,NewsSource,CategoryId,NewsStatus,TagIds")] NewsArticleToAddOrUpdateDTO newsArticle)
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
            newsArticle.NewsStatus = true;

            if (ModelState.IsValid)
            {
                result = await _newsArticleService.CreateNewsArticleAsync(newsArticle);
            }

            if (result) return RedirectToAction("Index", "NewsArticles");

            return View(newsArticle);
        }

        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _newsArticleService.GetAllCategories();
            var tags = await _newsArticleService.GetAllTags();
            var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);

            // Prepare comma-separated Tag IDs for ViewBag
            var selectedTagIds = string.Join(",", newsArticle.Tags.Select(tag => tag.Id.ToString()));

            ViewBag.Categories = categories;
            ViewBag.Tags = tags;
            ViewBag.SelectedCategoryId = newsArticle.CategoryId;
            ViewBag.SelectedTagIds = selectedTagIds;
            ViewData["Action"] = "Edit";

            var newsArticleToUpdate = new NewsArticleToAddOrUpdateDTO()
            {
                Headline = newsArticle.Headline,
                NewsTitle = newsArticle.NewsTitle,
                NewsContent = newsArticle.NewsContent,
                CategoryId = newsArticle.CategoryId,
                NewsStatus = newsArticle.NewsStatus,
                NewsSource = newsArticle.NewsSource,
                TagIds = selectedTagIds
            };

            if (newsArticle == null)
            {
                return NotFound();
            }
            
            return View(newsArticleToUpdate);
        }

        // POST: NewsArticles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsTitle,Headline,NewsContent,NewsSource,CategoryId,NewsStatus,UpdatedById,ModifiedDate,TagIds")] NewsArticleToAddOrUpdateDTO newsArticle)
        {
            if (ModelState.IsValid)
            {
                await _newsArticleService.UpdateNewsArticleAsync(id, newsArticle);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // POST: NewsArticles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _newsArticleService.DeleteNewsArticleAsync(id.Value);

            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
