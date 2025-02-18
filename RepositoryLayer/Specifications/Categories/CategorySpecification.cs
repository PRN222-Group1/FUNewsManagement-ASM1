using RepositoryLayer.Entities;

namespace RepositoryLayer.Specifications.Categories
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(CategorySpecParams specParams) : base(x => 
            (string.IsNullOrEmpty(specParams.Search) || x.CategoryName.Contains(specParams.Search))
            && (!specParams.CategoryId.HasValue || x.ParentCategoryId == specParams.CategoryId)
            && (!specParams.Status.HasValue || x.IsActive == specParams.Status)
        )
        {
            AddInclude(c => c.ParentCategory);
        }
    }
}
