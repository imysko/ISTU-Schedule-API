using System;
using System.Collections.Generic;

namespace API.DataBase.Models;

public partial class Discipline
{
    public int DisciplineId { get; set; }

    public string? Title { get; set; }

    public string? RealTitle { get; set; }

    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
