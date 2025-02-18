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
    private readonly IMapper _mapper;

    public CategoriesService(IGenericRepository<Category> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CategoryDTO>> GetCategoriesAsync(CategorySpecParams specParams)
    {
        var spec = new CategorySpecification(specParams);
        var categories = await _repository.ListAsync(spec);

        return _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryDTO>>(categories);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        bool result = false;
        var category = await _repository.GetByIdAsync(id);

        if (category == null)
        {
            return result;
        }

        category.IsActive = false;

        _repository.Update(category);
        result = await _repository.SaveAllAsync();

        return result;
    }


    public async Task<bool> EditCategoryAsync(int id, CategoryToAddOrUpdateDTO category)
    {
        bool result = false;
        var categoryToUpdate = await _repository.GetByIdAsync(id);

        if (categoryToUpdate == null)
        {
            return result;
        }

        categoryToUpdate.CategoryName = category.CategoryName;
        categoryToUpdate.CategoryDescription = category.CategoryDescription;
        categoryToUpdate.ParentCategoryId = category.CategoryId;
        categoryToUpdate.IsActive = category.Status;

        _repository.Update(categoryToUpdate);
        result = await _repository.SaveAllAsync();

        return result;
    }


    public async Task<bool> CreateCategoryAsync(CategoryToAddOrUpdateDTO category)
    {
        var result = false;
        var categoryToAdd = _mapper.Map<CategoryToAddOrUpdateDTO, Category>(category);

        _repository.Add(categoryToAdd);
        result = await _repository.SaveAllAsync();

        return result;
    }

    public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        return _mapper.Map<Category, CategoryDTO>(category);
    }
}