using BusinessServiceLayer.Interfaces;
using Group1MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Data;

namespace Group1MVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly FuNewsManagementContext _context;

        public ReportController(IReportService reportService, FuNewsManagementContext context)
        {

            _reportService = reportService;
            _context = context;
        }
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var newsArticles = await _reportService.GetNewsArticlesByDateRangeAsync(startDate, endDate);
            var count = await _reportService.GetCountNewsArticlesByDateRangeAsync(startDate, endDate);

            if (startDate.HasValue && endDate.HasValue)
            {
                if (startDate.Value > endDate.Value)
                {
                    TempData["ErrorMessage"] = "Start Date cannot be older than End Date";
                    return View(newsArticles);
                }
            }

            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;
            ViewData["Count"] = count;

            return View(newsArticles);
        }
    }
}
