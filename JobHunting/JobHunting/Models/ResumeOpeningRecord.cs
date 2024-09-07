using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class ResumeOpeningRecord
{
    public int ResumeOpeningRecordId { get; set; }

    public int? ResumeId { get; set; }

    public int? OpeningId { get; set; }

    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string OpeningTitle { get; set; } = null!;

    public DateOnly? ApplyDate { get; set; }

    public bool LikeYn { get; set; }

    public bool InterviewYn { get; set; }

    public bool HireYn { get; set; }

    public virtual Opening? Opening { get; set; }

    public virtual Resume? Resume { get; set; }
}
