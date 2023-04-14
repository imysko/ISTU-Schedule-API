using API.DataBase.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Data.Models;

[SwaggerSchema(Description = "Information about Lesson")]
public partial class Lesson
{
    [SwaggerSchema(Description = "lesson time")]
    public LessonsTime Time { get; set; }
    
    [SwaggerSchema(Description = "break time after this lesson")]
    public TimeSpan? BreakTimeAfter { get; set; }
    
    [SwaggerSchema(Description = "list of schedule in this time")]
    public virtual ICollection<Schedule> Schedules { get; set; }
}