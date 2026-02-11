using System;
using System.Collections.Generic;

namespace SchoolProject.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
