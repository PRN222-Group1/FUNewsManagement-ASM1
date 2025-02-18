namespace BusinessServiceLayer.DTOs
{
    public class CategoryToAddOrUpdateDTO
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public bool Status { get; set; }
    }
}
