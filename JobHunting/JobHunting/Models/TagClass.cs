using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class TagClass
{
    public int TagClassId { get; set; }

    public string TagClassName { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
