using System;
using System.Collections.Generic;

namespace SchoolProject.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public string GradeValue { get; set; } = null!;

    public DateOnly? DateGiven { get; set; }

    public string? StudentId { get; set; }

    public int? SubjectId { get; set; }

    public int? TeacherId { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Subject? Subject { get; set; }

    public virtual Staff? Teacher { get; set; }
}
