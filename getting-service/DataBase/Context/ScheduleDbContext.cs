﻿using Microsoft.EntityFrameworkCore;
using getting_service.DataBase.Models;

namespace getting_service.DataBase.Context;

public partial class ScheduleDbContext : DbContext
{
    private readonly string ConnectionString;
    
    public ScheduleDbContext(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Institute> Institutes { get; set; }

    public virtual DbSet<LessonsTime> LessonsTimes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=172.28.223.52;Port=5432;Database=schedule;Username=yaroslav;Password=5825");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.ClassroomId).HasName("classrooms_pkey");

            entity.ToTable("classrooms");

            entity.Property(e => e.ClassroomId).HasColumnName("classroom_id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.HasKey(e => e.DisciplineId).HasName("disciplines_pkey");

            entity.ToTable("disciplines");

            entity.Property(e => e.DisciplineId).HasColumnName("discipline_id");
            entity.Property(e => e.RealTitle).HasColumnName("real_title");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("groups_pkey");

            entity.ToTable("groups");

            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.Course).HasColumnName("course");
            entity.Property(e => e.InstituteId).HasColumnName("institute_id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Institute).WithMany(p => p.Groups)
                .HasForeignKey(d => d.InstituteId)
                .HasConstraintName("institute_id");
        });

        modelBuilder.Entity<Institute>(entity =>
        {
            entity.HasKey(e => e.InstituteId).HasName("institutes_pkey");

            entity.ToTable("institutes");

            entity.Property(e => e.InstituteId).HasColumnName("institute_id");
            entity.Property(e => e.InstituteTitle).HasColumnName("institute_title");
        });

        modelBuilder.Entity<LessonsTime>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("lessons_time_pkey");

            entity.ToTable("lessons_time");

            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.BegTime).HasColumnName("begtime");
            entity.Property(e => e.EndTime).HasColumnName("endtime");
            entity.Property(e => e.LessonNumber).HasColumnName("lesson_number");
        });

        modelBuilder.Entity<ScheduleGroup>(entity =>
        {
            entity.ToTable("schedule_groups");

            entity.HasKey(e => new { e.ScheduleId, e.GroupId });

            entity
                .HasOne(e => e.Schedule)
                .WithMany(e => e.ScheduleGroups)
                .HasForeignKey(e => e.ScheduleId)
                .HasConstraintName("schedule_groups_schedule_null_fk");
            
            entity
                .HasOne(e => e.Group)
                .WithMany(e => e.ScheduleGroups)
                .HasForeignKey(e => e.GroupId)
                .HasConstraintName("schedule_groups_groups_null_fk");
        });
        
        modelBuilder.Entity<ScheduleTeacher>(entity =>
        {
            entity.ToTable("schedule_teachers");

            entity.HasKey(e => new { e.ScheduleId, e.TeacherId });

            entity
                .HasOne(e => e.Schedule)
                .WithMany(e => e.ScheduleTeachers)
                .HasForeignKey(e => e.ScheduleId)
                .HasConstraintName("schedule_teachers_schedule_null_fk");
            
            entity
                .HasOne(e => e.Teacher)
                .WithMany(e => e.ScheduleTeachers)
                .HasForeignKey(e => e.TeacherId)
                .HasConstraintName("schedule_teachers_teachers_null_fk");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("schedule_pkey");

            entity.ToTable("schedule");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.ClassroomId).HasColumnName("classroom_id");
            entity.Property(e => e.ClassroomVerbose).HasColumnName("classroom_verbose");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DisciplinesId).HasColumnName("discipline_id");
            entity.Property(e => e.DisciplineVerbose).HasColumnName("discipline_verbose");
            entity.Property(e => e.GroupsVerbose).HasColumnName("groups_verbose");
            entity.Property(e => e.LessonId).HasColumnName("lesson_id");
            entity.Property(e => e.LessonType).HasColumnName("lesson_type");
            entity.Property(e => e.Subgroup).HasColumnName("subgroup");
            entity.Property(e => e.TeachersVerbose).HasColumnName("teachers_verbose");

            entity.HasOne(d => d.Classroom).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.ClassroomId)
                .HasConstraintName("schedule_classrooms_null_fk");

            entity.HasOne(d => d.Discipline).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.DisciplinesId)
                .HasConstraintName("discipline_fk");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("lesson_time_fk");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("teachers_pkey");

            entity.ToTable("teachers");

            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Fullname).HasColumnName("fullname");
            entity.Property(e => e.Shortname).HasColumnName("shortname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
