using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

public partial class ScheduleGroup
{
    [Column("schedule_id")]
    [SwaggerSchema(Description = "schedule Id")]
    public int ScheduleId { get; set; }
    
    [SwaggerSchema(Description = "information about schedule")]
    public Schedule Schedule { get; set; }

    [Column("group_id")]
    [SwaggerSchema(Description = "group Id")]
    public int GroupId { get; set; }
    
    [SwaggerSchema(Description = "information about group")]
    public Group Group { get; set; }
}