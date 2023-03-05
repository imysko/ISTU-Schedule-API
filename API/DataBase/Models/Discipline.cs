using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about academic discipline")]
public partial class Discipline
{
    [SwaggerSchema(Description = "discipline Id")]
    public int DisciplineId { get; set; }

    [SwaggerSchema(Description = "name of the discipline")]
    public string? Title { get; set; }

    [SwaggerSchema(Description = "full name of the discipline")]
    public string? RealTitle { get; set; }

    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
