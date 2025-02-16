namespace BusinessServiceLayer.DTOs;

public class CategoriesDTO
{
    public int ParentCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? CategoryDescription { get; set; }
    public bool IsActive { get; set; }
}