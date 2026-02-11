using System;
using System.Collections.Generic;

namespace SchoolProject.Models;

public partial class Student
{
    public string StudentId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? ClassId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
