using System;
using System.Collections.Generic;

namespace JobHunting.Models;

public partial class OpinionLetter
{
    public int LetterId { get; set; }

    public int? CompanyId { get; set; }

    public int? CandidateId { get; set; }

    public int? AdminId { get; set; }

    public string Class { get; set; } = null!;

    public string SubjectLine { get; set; } = null!;

    public string Content { get; set; } = null!;

    public byte[]? Attachment { get; set; }

    public bool Status { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Candidate? Candidate { get; set; }

    public virtual Company? Company { get; set; }
}
