using AutoMapper;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Specifications.NewsArticles;

namespace BusinessServiceLayer.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NewsArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<NewsArticleDTO>> GetNewsArticlesAsync(NewsArticleSpecParams specParams)
        {
            specParams.Status = true;
            var spec = new NewsArticleSpecification(specParams);
            var newsArticles = await _unitOfWork.Repository<NewsArticle>().ListAsync(spec);
            return _mapper.Map<IReadOnlyList<NewsArticle>, IReadOnlyList<NewsArticleDTO>>(newsArticles);
        }

        public async Task<int> CountNewsArticlesAsync(NewsArticleSpecParams specParams)
        {
            specParams.Status = true;
            var spec = new NewsArticleCountSpecification(specParams);
            var count = await _unitOfWork.Repository<NewsArticle>().CountAsync(spec);
            return count;
        }

        public async Task<NewsArticleDTO> GetNewsArticleByIdAsync(int newsId)
        {
            var spec = new NewsArticleSpecification(newsId);
            var newsArticle = await _unitOfWork.Repository<NewsArticle>().GetEntityWithSpec(spec);
            return _mapper.Map<NewsArticle, NewsArticleDTO>(newsArticle);
        }

        public async Task<IReadOnlyList<CategoryDTO>> GetAllCategories()
        {
            var categories = await _unitOfWork.Repository<Category>().ListAllAsync();
            return _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDTO>>(categories);
        }
    }
}
