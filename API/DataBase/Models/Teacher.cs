using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about teacher")]
public partial class Teacher
{
    [SwaggerSchema(Description = "teacher Id")]
    public int TeacherId { get; set; }

    [SwaggerSchema(Description = "full name of the teacher")]
    public string? Fullname { get; set; }

    [SwaggerSchema(Description = "short name of the teacher")]
    public string? Shortname { get; set; }
    
    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<ScheduleTeacher> ScheduleTeachers { get; set; }
    
    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<Schedule> Schedules { get; set; }
}
