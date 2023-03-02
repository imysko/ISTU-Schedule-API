using System;
using System.Collections.Generic;

namespace API.DataBase.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string? Name { get; set; }

    public int? Course { get; set; }

    public int? InstituteId { get; set; }

    public virtual Institute? Institute { get; set; }
    
    public virtual ICollection<ScheduleGroup> ScheduleGroups { get; set; }
}
