using BusinessServiceLayer.DTOs;
using RepositoryLayer.Specifications.NewsArticles;

namespace BusinessServiceLayer.Interfaces
{
    public interface INewsArticleService
    {
        Task<IReadOnlyList<NewsArticleDTO>> GetNewsArticlesAsync(NewsArticleSpecParams search);
        Task<int> CountNewsArticlesAsync(NewsArticleSpecParams specParams);
        Task<NewsArticleDTO> GetNewsArticleByIdAsync(int newsId);
        Task<IReadOnlyList<CategoryDTO>> GetAllCategories();
        Task<bool> CreateNewsArticle(NewsArticleToAddDTO newsArticle);
        Task<IReadOnlyList<TagDTO>> GetAllTags();
    }
}
