using System;
using System.Collections.Generic;

namespace API.DataBase.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string? Fullname { get; set; }

    public string? Shortname { get; set; }
}
