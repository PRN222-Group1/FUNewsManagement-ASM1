﻿using BusinessServiceLayer.DTOs;
using RepositoryLayer.Specifications.Categories;

namespace BusinessServiceLayer.Interfaces;

public interface ICategoriesService
{
    Task<IReadOnlyList<CategoryDTO>> GetCategoriesAsync(CategorySpecParams specParams);
    Task<CategoryDTO> GetCategoryByIdAsync(int id);

    Task<bool> DeleteCategoryAsync(int id);

    Task<bool> EditCategoryAsync(int id, CategoryToAddOrUpdateDTO category);

    Task<bool> CreateCategoryAsync(CategoryToAddOrUpdateDTO category);
}