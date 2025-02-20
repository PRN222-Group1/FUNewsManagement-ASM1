﻿namespace RepositoryLayer.Specifications.Categories
{
    public class CategorySpecParams
    {
        public int? CategoryId { get; set; } = null;
        public string Sort { get; set; }
        public bool? Status { get; set; }

        private string _search;

        public string Search
        {
            get => _search;
            set => _search = (value != null) ? value.ToLower() : "";
        }
    }
}
