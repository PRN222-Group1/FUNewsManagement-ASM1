﻿using AutoMapper;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Specifications.Categories;
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
            var spec = new NewsArticleSpecification(specParams);
            var newsArticles = await _unitOfWork.Repository<NewsArticle>().ListAsync(spec);
            return _mapper.Map<IReadOnlyList<NewsArticle>, IReadOnlyList<NewsArticleDTO>>(newsArticles);
        }

        public async Task<int> CountNewsArticlesAsync(NewsArticleSpecParams specParams)
        {
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
            var specParams = new CategorySpecParams();
            specParams.Status = true;
            var spec = new CategorySpecification(specParams);
            var categories = await _unitOfWork.Repository<Category>().ListAsync(spec);
            return _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDTO>>(categories);
        }

        public async Task<NewsArticleDTO> GetNewsArticleByStaffId(int staffId)
        {
            var spec = new NewsArticleSpecification(staffId);
            var newsArticle = await _unitOfWork.Repository<NewsArticle>().GetEntityWithSpec(spec);
            return _mapper.Map<NewsArticle, NewsArticleDTO>(newsArticle);
        }

        public async Task<bool> CreateNewsArticleAsync(NewsArticleToAddOrUpdateDTO newsArticle)
        {
            // Split the TagIds string by commas and convert to a list of integers
            var tagIdList = newsArticle.TagIds
                .Split(',')
                .Select(id => int.Parse(id))
                .ToList();

            // Create a in-memory list of tags
            var tagList = new List<Tag>();

            foreach (int tagId in tagIdList)
            {
                var tag = await _unitOfWork.Repository<Tag>().GetByIdAsync(tagId);
                tagList.Add(tag);
            }

            var newsArticleToAdd = _mapper.Map<NewsArticleToAddOrUpdateDTO, NewsArticle>(newsArticle);
            newsArticleToAdd.Tags = tagList;

            // Add news article and save changes to the database
            _unitOfWork.Repository<NewsArticle>().Add(newsArticleToAdd);
            var result = await _unitOfWork.Complete();
            return result;
        }

        public async Task<bool> UpdateNewsArticleAsync(int id, NewsArticleToAddOrUpdateDTO newsArticle)
        {
            // Split the TagIds string by commas and convert to a list of integers
            var tagIdList = newsArticle.TagIds
                .Split(',')
                .Select(id => int.Parse(id))
                .ToList();

            // Create a in-memory list of tags
            var tagList = new List<Tag>();

            foreach (int tagId in tagIdList)
            {
                var tag = await _unitOfWork.Repository<Tag>().GetByIdAsync(tagId);
                tagList.Add(tag);
            }

            var spec = new NewsArticleSpecification(id);
            var newsArticleToUpdate = await _unitOfWork.Repository<NewsArticle>().GetEntityWithSpec(spec);
            
            newsArticleToUpdate.NewsTitle = newsArticle.NewsTitle;
            newsArticleToUpdate.Headline = newsArticle.Headline;
            newsArticleToUpdate.CategoryId = newsArticle.CategoryId;
            newsArticleToUpdate.NewsContent = newsArticle.NewsContent;
            newsArticleToUpdate.NewsStatus = newsArticle.NewsStatus;
            newsArticleToUpdate.UpdatedById = newsArticle.UpdatedById;
            newsArticleToUpdate.ModifiedDate = DateTime.UtcNow;
            newsArticleToUpdate.Tags = tagList;

            // Add news article and save changes to the database
            _unitOfWork.Repository<NewsArticle>().Update(newsArticleToUpdate);
            var result = await _unitOfWork.Complete();
            return result;
        }

        public async Task<bool> DeleteNewsArticleAsync(int id)
        {
            var spec = new NewsArticleSpecification(id);
            var newsArticleToDelete = await _unitOfWork.Repository<NewsArticle>().GetEntityWithSpec(spec);

            if (newsArticleToDelete == null) return false;

            newsArticleToDelete.NewsStatus = false;

            _unitOfWork.Repository<NewsArticle>().Update(newsArticleToDelete);
            var result = await _unitOfWork.Complete();
            return result;
        }

        public async Task<IReadOnlyList<TagDTO>> GetAllTags()
        {
            var tags = await _unitOfWork.Repository<Tag>().ListAllAsync();

            return _mapper.Map<IReadOnlyList<Tag>, IReadOnlyList<TagDTO>>(tags);
        }

        public async Task<IReadOnlyList<NewsArticleDTO>> GetNewsArticleHistoryAsync(int accountId)
        {
            var spec = new NewsArticleHistorySpecification(accountId);
            var newsArticles = await _unitOfWork.Repository<NewsArticle>().ListAsync(spec);
            return _mapper.Map<IReadOnlyList<NewsArticle>, IReadOnlyList<NewsArticleDTO>>(newsArticles);
        }
    }
}
