using System;
using System.Collections.Generic;

namespace API.DataBase.Models;

public partial class LessonsTime
{
    public string? LessonNumber { get; set; }

    public string? Begtime { get; set; }

    public string? Endtime { get; set; }

    public int LessonId { get; set; }

    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
