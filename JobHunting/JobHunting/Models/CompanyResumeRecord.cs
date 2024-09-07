using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class CompanyResumeRecord
{
    public int CompanyId { get; set; }

    public int ResumeId { get; set; }

    public bool LikeYn { get; set; }

    public bool? InterviewYn { get; set; }

    public bool? HireYn { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Resume Resume { get; set; } = null!;
}
