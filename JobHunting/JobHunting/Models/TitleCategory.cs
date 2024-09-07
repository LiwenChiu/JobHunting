using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class TitleCategory
{
    public string TitleCategoryId { get; set; } = null!;

    public string TitleCategoryName { get; set; } = null!;

    public virtual ICollection<TitleClass> TitleClasses { get; set; } = new List<TitleClass>();
}
