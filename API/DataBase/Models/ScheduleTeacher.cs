using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

public partial class ScheduleTeacher
{
    [Column("schedule_id")]
    [SwaggerSchema(Description = "schedule Id")]
    public int ScheduleId { get; set; }
    
    [SwaggerSchema(Description = "information about schedule")]
    public Schedule Schedule { get; set; }

    [Column("teacher_id")]
    [SwaggerSchema(Description = "teacher Id")]
    public int TeacherId { get; set; }
    
    [SwaggerSchema(Description = "information about teacher")]
    public Teacher Teacher { get; set; }
}