namespace BusinessServiceLayer.DTOs;

public class NewsArticlesDTO
{
    public int Id { get; set; }
    public string NewsTitle { get; set; }
    public string Headline { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string NewsContent { get; set; }
    public string NewsSource { get; set; }
    public int? CategoryId { get; set; }
    public bool? NewsStatus { get; set; }
    public int? CreatedById { get; set; }
    public int? UpdatedById { get; set; }
    public DateTime? ModifiedDate { get; set; }

    // Thông tin danh mục
    public string CategoryName { get; set; }
    public string CategoryDescription { get; set; }
    public int? ParentCategoryId { get; set; }
    public bool IsActive { get; set; }

    // Thông tin tài khoản
    public string AccountName { get; set; }
    public string AccountEmail { get; set; }
    public int AccountRole { get; set; }
    public string AccountPassword { get; set; }
    
}