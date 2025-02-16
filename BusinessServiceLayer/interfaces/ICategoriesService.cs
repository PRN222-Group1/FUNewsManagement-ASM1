using BusinessServiceLayer.DTOs;
using RepositoryLayer.Specifications.Categories;

namespace BusinessServiceLayer.Interfaces;

public interface ICategoriesService
{
    /*
     * get all categories
     */
    Task<List<CategoriesDTO>> getAll();
    
    Task<List<NewsArticlesDTO>> getNewsArticlesByID(int idCategory);

    Task<string> deleteCategory(int idcategory);

    Task<string> editCategory(int idcategory, EditCategoriesentity editCategoriesentity);

    Task<string> createCategory(EditCategoriesentity createCategoriesEntity);
    Task<List<CategoriesDTO>> searchCategory(string keyword);
}