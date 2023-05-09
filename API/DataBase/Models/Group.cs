using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about group")]
public partial class Group
{
    [SwaggerSchema(Description = "group Id")]
    public int GroupId { get; set; }

    [SwaggerSchema(Description = "name of the group")]
    public string? Name { get; set; }

    [SwaggerSchema(Description = "course number of the group")]
    public int? Course { get; set; }

    [SwaggerSchema(Description = "institute Id of the group")]
    public int? InstituteId { get; set; }
    
    public bool? IsActive { get; set; }

    [SwaggerSchema(Description = "information about institute")]
    public virtual Institute? Institute { get; set; }
    
    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<ScheduleGroup> ScheduleGroups { get; set; }
    
    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<Schedule> Schedules { get; set; }
}
