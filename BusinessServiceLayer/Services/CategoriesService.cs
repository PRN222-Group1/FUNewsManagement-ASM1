using AutoMapper;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Specifications.Categories;

namespace BusinessServiceLayer.Services;

public class CategoriesService : ICategoriesService
{
    private readonly IGenericRepository<Category> _repository;

    public CategoriesService(IGenericRepository<Category> repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoriesDTO>> getAll()
    {
        var categories = await _repository.GetAll();
        List<CategoriesDTO> categoriesDtos = categories.Select(category => new CategoriesDTO()
        {
            CategoryDescription = category.CategoryDescription,
            IsActive = category.IsActive ?? false,
            ParentCategoryId = category.ParentCategoryId ?? 0,
            CategoryName = category.CategoryName ?? "No Name"
        }).ToList();
        return categoriesDtos;
    }

    public async Task<List<NewsArticlesDTO>> getNewsArticlesByID(int idCategory)
    {
        Category category = await _repository.GetByIdAsync(idCategory);

        if (category == null)
            throw new Exception("Category not found");

        List<NewsArticle> newsArticles = category.NewsArticles.ToList();

        List<NewsArticlesDTO> newsDTOs = newsArticles.Select(news => new NewsArticlesDTO
        {
            NewsTitle = news.NewsTitle ?? "No Title",
            Headline = news.Headline,
            CreatedDate = news.CreatedDate ?? DateTime.MinValue,
            NewsContent = news.NewsContent ?? "No Content",
            NewsSource = news.NewsSource ?? "N/A",
            CategoryId = news.CategoryId ?? 0,
            NewsStatus = news.NewsStatus ?? false,
            CreatedById = news.CreatedById ?? 0,
            UpdatedById = news.UpdatedById ?? 0,
            ModifiedDate = news.ModifiedDate ?? DateTime.MinValue,

            CategoryName = category.CategoryName,
            CategoryDescription = category.CategoryDescription,
            ParentCategoryId = category.ParentCategoryId,
            IsActive = category.IsActive ?? false,

            AccountName = news.CreatedBy?.AccountName ?? "Unknown",
            AccountEmail = news.CreatedBy?.AccountEmail ?? "Unknown",
            AccountRole = news.CreatedBy?.AccountRole ?? 0,
            AccountPassword = "Hidden"
        }).ToList();

        return newsDTOs;
    }

    public async Task<string> deleteCategory(int idcategory)
    {
        Category category = await _repository.GetByIdAsync(idcategory);

        if (category == null)
        {
            return "Category not found";
        }

        if (category.NewsArticles.Count == 0)
        {
            category.IsActive = false;
            _repository.Update(category);
            await _repository.SaveChangesAsync();

            return "Delete Category successfully";
        }

        return "Delete Category failed: Category still contains news articles.";
    }


    public async Task<string> editCategory(int idcategory, EditCategoriesentity editCategoriesentity)
    {
        Category category = await _repository.GetByIdAsync((int)editCategoriesentity.ParentCategoryId);

        if (category == null || category.IsActive == false)
        {
            return "Edit category failed";
        }

        category.CategoryName = editCategoriesentity.CategoryName;
        category.CategoryDescription = editCategoriesentity.CategoryDescription;

        _repository.Update(category);
        await _repository.SaveChangesAsync();

        return "Edit category successfully";
    }


    public async Task<string> createCategory(EditCategoriesentity createCategoriesEntity)
    {
        var category = new Category
        {
            CategoryName = createCategoriesEntity.CategoryName,
            CategoryDescription = createCategoriesEntity.CategoryDescription,
            IsActive = true
        };

        _repository.Add(category);
        await _repository.SaveChangesAsync();

        return "Create category successfully";
    }

    public async Task<List<CategoriesDTO>> searchCategory(string keyword)
    {
        var categories = await _repository.GetAll();

        var filteredCategories = categories
            .Where(c => c.CategoryName.ToLower().Contains(keyword.ToLower()) ||
                        c.CategoryDescription.ToLower().Contains(keyword.ToLower()) ||
                        (c.ParentCategoryId.HasValue && c.ParentCategoryId.ToString() == keyword))
            .ToList();

        List<CategoriesDTO> categoriesDtos = filteredCategories.Select(c => new CategoriesDTO
        {
            ParentCategoryId = (int)c.ParentCategoryId,
            CategoryName = c.CategoryName,
            CategoryDescription = c.CategoryDescription,
            IsActive = (bool)c.IsActive,
        }).ToList();

        return categoriesDtos;
    }
}