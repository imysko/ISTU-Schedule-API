using System;
using System.Collections.Generic;

namespace API.DataBase.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public string? GroupsVerbose { get; set; }

    public string? TeachersVerbose { get; set; }

    public int? ClassroomId { get; set; }

    public string? ClassroomVerbose { get; set; }

    public int? DisciplineId { get; set; }

    public string? DisciplineVerbose { get; set; }

    public int? LessonId { get; set; }

    public int? Subgroup { get; set; }

    public int? LessonType { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Classroom? Classroom { get; set; }

    public virtual Discipline? Discipline { get; set; }

    public virtual LessonsTime? Lesson { get; set; }
}
