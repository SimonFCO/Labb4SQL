using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SchoolProject.Models;

public partial class SchoolDatabaseContext : DbContext
{
    public SchoolDatabaseContext()
    {
    }

    public SchoolDatabaseContext(DbContextOptions<SchoolDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=SchoolDatabase;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Class__CB1927C08A2C3666");

            entity.ToTable("Class");

            entity.Property(e => e.ClassId).ValueGeneratedNever();
            entity.Property(e => e.ClassName)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Mentor).WithMany(p => p.Classes)
                .HasForeignKey(d => d.MentorId)
                .HasConstraintName("FK__Class__MentorId__52593CB8");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BEDF33CBFEF");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grade__54F87A579246A9C7");

            entity.ToTable("Grade");

            entity.Property(e => e.DateGiven).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.GradeValue)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StudentId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Grade__StudentId__5AEE82B9");

            entity.HasOne(d => d.Subject).WithMany(p => p.Grades)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__Grade__SubjectId__5BE2A6F2");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Grades)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Grade__TeacherId__5CD6CB2B");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A8C2CA643");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB1756974E09");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Department).WithMany(p => p.Staff)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Staff__Departmen__4F7CD00D");

            entity.HasOne(d => d.Role).WithMany(p => p.Staff)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Staff__RoleId__4E88ABD4");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52B99C3DA7939");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Student__ClassId__5535A963");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subject__AC1BA3A82F7DC1FC");

            entity.ToTable("Subject");

            entity.Property(e => e.SubjectId).ValueGeneratedNever();
            entity.Property(e => e.SubjectName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
