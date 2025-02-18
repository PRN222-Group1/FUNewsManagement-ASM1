namespace BusinessServiceLayer.DTOs
{
    public class NewsArticleToAddOrUpdateDTO
    {
        public string? NewsTitle { get; set; }

        public string Headline { get; set; } = null!;

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public int? CategoryId { get; set; }

        public bool NewsStatus { get; set; } = true;

        public int? CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;

        public string? TagIds { get; set; }
    }
}
