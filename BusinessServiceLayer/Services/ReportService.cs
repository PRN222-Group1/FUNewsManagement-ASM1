using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;

namespace BusinessServiceLayer.Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<NewsArticle> _repository;
        private readonly FuNewsManagementContext _context;

        public ReportService(IGenericRepository<NewsArticle> repository, FuNewsManagementContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<NewsArticlesDTO>> GetNewsArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var articles = await _context.NewsArticles
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderBy(n => n.CreatedDate)
                 .Include(n => n.Category)
        .Include(n => n.CreatedBy)

                .ToListAsync();

            // Project dữ liệu sang DTO (có thể bao gồm cả dữ liệu liên quan như Tags)
            List<NewsArticlesDTO> newarticleDtos = articles.Select(article => new NewsArticlesDTO()
            {
                Id = article.Id,
                NewsTitle = article.NewsTitle,
                Headline = article.Headline,
                CreatedDate = article.CreatedDate,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource,
                NewsStatus = article.NewsStatus,
                CreatedById = article.CreatedById,
                UpdatedById = article.UpdatedById,
                ModifiedDate = article.ModifiedDate,
                CategoryName = article.Category != null ? article.Category.CategoryName : string.Empty,
            }).ToList();
            return newarticleDtos;
        }

        public async Task<List<NewsArticlesDTO>> GetAllNewsArticlesAsync()
        {
            var articles = await _repository.GetAll();
            List<NewsArticlesDTO> newarticleDtos = articles.Select(article => new NewsArticlesDTO()
            {
                NewsTitle = article.NewsTitle,
                Headline = article.Headline,
                CreatedDate = article.CreatedDate,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource,
                CategoryId = article.CategoryId,
                NewsStatus = article.NewsStatus,
                CreatedById = article.CreatedById,
                UpdatedById = article.UpdatedById,
                ModifiedDate = article.ModifiedDate,
            }).ToList();
            return newarticleDtos;
        }
        public async Task<List<NewsArticlesDTO>> GetNewsArticlesByCreatedByIdAsync(int createdById)
        {
            var articles = await _context.NewsArticles
                .Where(n => n.CreatedById == createdById)
                .OrderByDescending(n => n.CreatedDate)
                .Include(n => n.Category)    // Load thông tin danh mục nếu cần
                .Include(n => n.CreatedBy)   // Load thông tin tài khoản nếu cần
                .ToListAsync();

            List<NewsArticlesDTO> articleDtos = articles.Select(article => new NewsArticlesDTO
            {
                Id = article.Id,  // Mapping thuộc tính Id từ BaseEntity
                NewsTitle = article.NewsTitle,
                Headline = article.Headline,
                CreatedDate = article.CreatedDate,
                NewsContent = article.NewsContent,
                NewsSource = article.NewsSource,
                CategoryId = article.CategoryId,
                NewsStatus = article.NewsStatus,
                CreatedById = article.CreatedById,
                UpdatedById = article.UpdatedById,
                ModifiedDate = article.ModifiedDate,
            }).ToList();
            return articleDtos;
        }
    }
}