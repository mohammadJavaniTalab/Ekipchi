﻿using System.Collections.Generic;

namespace Hastnama.Ekipchi.Data.BlogCategory
{
    public class BlogCategoryDto
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Cover { get; set; }

        public string SlugPath { get; set; }

        public string Description { get; set; }

        public string LongDescription { get; set; }

        public string Logo { get; set; }

        public int SortOrder { get; set; }

        public bool IsDeleted { get; set; }

        public BlogCategoryDto ParentCategory { get; set; }

        public IList<BlogCategoryDto> Children { get; }
    }
}