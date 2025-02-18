using RepositoryLayer.Entities;

namespace BusinessServiceLayer.DTOs
{
    public class SystemAccountDTO
    {
        public int? Id { get; set; }
        public string? AccountName { get; set; }

        public string? AccountEmail { get; set; }

        public int? AccountRole { get; set; }
    }
}
