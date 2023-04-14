using API.Data.Enums;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about other discipline")]
public partial class OtherDiscipline
{
    [SwaggerSchema(Description = "other discipline Id")]
    public int OtherDisciplineId { get; set; }

    [SwaggerSchema(Description = "name of the discipline")]
    public string? DisciplineTitle { get; set; }

    [SwaggerSchema(Description = "is this discipline conducted online")]
    public bool? IsOnline { get; set; }
    
    [SwaggerSchema(Description = "type of the discipline")]
    public OtherDisciplineType? Type { get; set; }

    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}