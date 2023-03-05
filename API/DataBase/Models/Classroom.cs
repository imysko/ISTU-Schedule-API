using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about classroom")]
public partial class Classroom
{
    [SwaggerSchema(Description = "classroom Id")]
    public int ClassroomId { get; set; }

    [SwaggerSchema(Description = "name of classroom")]
    public string? Name { get; set; }

    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
