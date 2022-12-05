using System;
using System.Collections.Generic;

namespace API.DataBase.Models;

public partial class Institute
{
    public int InstituteId { get; set; }

    public string? InstituteTitle { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
