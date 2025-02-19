using BusinessServiceLayer.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Group1MVC.ViewModels
{
    public class NewsArticleSearchByCreatedIdViewModel
    {
        
        [Display(Name = "Created By ID")]
        [Required(ErrorMessage = "Vui lòng nhập Created By ID")]
        public int? CreatedById { get; set; }

        public List<NewsArticlesDTO>? Results { get; set; }
    }
}
