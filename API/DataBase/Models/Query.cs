using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about query")]
public class Query
{
    [SwaggerSchema(Description = "query Id")]
    public int QueryId { get; set; }

    [SwaggerSchema(Description = "description of the query")]
    public string? Description { get; set; }

    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}