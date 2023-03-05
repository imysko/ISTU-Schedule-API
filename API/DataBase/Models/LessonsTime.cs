using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about lesson time")]
public partial class LessonsTime
{
    [SwaggerSchema(Description = "lesson Id")]
    public int LessonId { get; set; }
    
    [SwaggerSchema(Description = "sequence number of the lesson")]
    public string? LessonNumber { get; set; }

    [SwaggerSchema(Description = "lesson start time")]
    public string? Begtime { get; set; }

    [SwaggerSchema(Description = "lesson end time")]
    public string? Endtime { get; set; }

    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
