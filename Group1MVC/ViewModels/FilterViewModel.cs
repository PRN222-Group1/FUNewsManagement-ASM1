﻿using BusinessServiceLayer.DTOs;

namespace Group1MVC.ViewModels
{
    public class SortOption
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class FilterViewModel
    {
        public string SearchPlaceholder { get; set; }
        public string SearchQuery { get; set; }
        public string SelectedSortOption { get; set; }
        public int? SelectedCategory { get; set; } = null;
        public List<SortOption> SortOptions { get; set; }
        public IReadOnlyList<CategoryDTO> Categories { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
}
