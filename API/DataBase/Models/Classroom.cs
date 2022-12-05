using System;
using System.Collections.Generic;

namespace API.DataBase.Models;

public partial class Classroom
{
    public int ClassroomId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
