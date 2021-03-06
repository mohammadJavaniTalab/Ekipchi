﻿using System;
using System.Text.Json.Serialization;
using Hastnama.Ekipchi.Common.Enum;

namespace Hastnama.Ekipchi.Data.Blog
{
    public class UpdateBlogDto
    {
        [JsonIgnore] public int Id { get; set; }

        public int BlogCategoryId { get; set; }

        public string BlogCategoryName { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public string MetaKeyWord { get; set; }

        public string Slug { get; set; }

        public string ShortDescription { get; set; }

        public string Image { get; set; }

        public string Tag { get; set; }

        public string Link { get; set; }

        public bool IsPublish { get; set; }

        public ContentType ContentType { get; set; }

        public VideoType? VideoType { get; set; }

        public DateTime? PublishDate { get; set; }

        public bool IsSpecial { get; set; }
    }
}