using BusinessServiceLayer.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Group1MVC.ViewModels
{
    public class NewsArticleSearchViewModel
    {
        [Required(ErrorMessage = "Chọn ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Chọn ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public IEnumerable<NewsArticleDTO>? Results { get; set; }
    }
}

