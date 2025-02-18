using AutoMapper;
using BusinessServiceLayer.DTOs;
using RepositoryLayer.Entities;

namespace Group1MVC.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<SystemAccount, SystemAccountDTO>();
            CreateMap<Tag, TagDTO>();
            CreateMap<Category, CategoryDTO>()
                .ForMember(c => c.ParentCategoryName, o => o.MapFrom(s => s.ParentCategory.CategoryName));
            CreateMap<CategoryToAddOrUpdateDTO, Category>()
                .ForMember(c => c.ParentCategoryId, o => o.MapFrom(s => s.CategoryId))
                .ForMember(c => c.IsActive, o => o.MapFrom(s => s.Status));
            CreateMap<NewsArticleToAddOrUpdateDTO, NewsArticle>();
            CreateMap<NewsArticle, NewsArticleDTO>()
                .ForMember(na => na.AuthorName, o => o.MapFrom(s => s.CreatedBy.AccountName))
                .ForMember(na => na.Tags, o => o.MapFrom(s => s.Tags))
                .ForMember(na => na.CategoryName, o => o.MapFrom(s => s.Category.CategoryName));
        }
    }
}
