using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public int PersonnelCode { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? Authority { get; set; }

    public virtual ICollection<OpinionLetter> OpinionLetters { get; set; } = new List<OpinionLetter>();
}
