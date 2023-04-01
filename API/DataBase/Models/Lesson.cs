using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about Lesson")]
public partial class Lesson
{
    [SwaggerSchema(Description = "lesson time")]
    public LessonsTime Time { get; set; }
    
    [SwaggerSchema(Description = "list of schedule in this time")]
    public virtual ICollection<Schedule> Schedules { get; set; }
}