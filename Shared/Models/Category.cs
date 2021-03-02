using System;
using System.Collections.Generic;
using System.Text;

namespace StoryForce.Shared.Models
{
    public class Category : Filter
    {
        public override string Type { get; } = "Category";

        public Category ParentCategory { get; set; }

        public IList<Category> SubCategories { get; set; }
    }
}
