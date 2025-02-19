using BusinessServiceLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServiceLayer.Interfaces
{
    public interface IReportService
    {
        Task<List<NewsArticlesDTO>> GetNewsArticlesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<NewsArticlesDTO>> GetAllNewsArticlesAsync();

        Task<List<NewsArticlesDTO>> GetNewsArticlesByCreatedByIdAsync(int createdById);
    }
}
