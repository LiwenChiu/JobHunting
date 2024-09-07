using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class TitleClass
{
    public string TitleClassId { get; set; } = null!;

    public string TitleCategoryId { get; set; } = null!;

    public string TitleClassName { get; set; } = null!;

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Opening> Openings { get; set; } = new List<Opening>();

    public virtual ICollection<Resume> Resumes { get; set; } = new List<Resume>();

    public virtual TitleCategory TitleCategory { get; set; } = null!;
}
