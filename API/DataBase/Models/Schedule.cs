using API.Data.Enums;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about schedule")]
public partial class Schedule
{
    [SwaggerSchema(Description = "schedule Id")]
    public int ScheduleId { get; set; }
    
    [SwaggerSchema(Description = "text description of groups")]
    public string? GroupsVerbose { get; set; }

    [SwaggerSchema(Description = "text description of teachers")]
    public string? TeachersVerbose { get; set; }

    [SwaggerSchema(Description = "classroom Id")]
    public int? ClassroomId { get; set; }

    [SwaggerSchema(Description = "text description of classroom")]
    public string? ClassroomVerbose { get; set; }

    [SwaggerSchema(Description = "discipline Id")]
    public int? DisciplineId { get; set; }

    [SwaggerSchema(Description = "text description of discipline")]
    public string? DisciplineVerbose { get; set; }
    
    [SwaggerSchema(Description = "other discipline Id")]
    public int? OtherDisciplineId { get; set; }
    
    [SwaggerSchema(Description = "query Id")]
    public int? QueryId { get; set;}

    [SwaggerSchema(Description = "lesson time Id")]
    public int? LessonId { get; set; }

    [SwaggerSchema(Description = "number of the subgroup")]
    public Subgroup? Subgroup { get; set; }

    [SwaggerSchema(Description = "type of the lesson")]
    public LessonType? LessonType { get; set; }
    
    [SwaggerSchema(Description = "type of the schedule")]
    public string? ScheduleType { get; set; }

    [SwaggerSchema(Description = "date of the lesson")]
    public DateOnly Date { get; set; }

    [SwaggerSchema(Description = "information about classroom")]
    public virtual Classroom? Classroom { get; set; }

    [SwaggerSchema(Description = "information about discipline")]
    public virtual Discipline? Discipline { get; set; }
    
    [SwaggerSchema(Description = "information about other discipline")]
    public virtual OtherDiscipline? OtherDiscipline { get; set; }
    
    [SwaggerSchema(Description = "information about query")]
    public virtual Query? Query { get; set; }

    [SwaggerSchema(Description = "information about lesson time")]
    public virtual LessonsTime? LessonTime { get; set; }
    
    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<Query> ReplacementQueries { get; set; }
    
    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<ScheduleGroup> ScheduleGroups { get; set; }
    
    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<ScheduleTeacher> ScheduleTeachers { get; set; }
    
    [SwaggerSchema(Description = "list of groups")]
    public virtual ICollection<Group> Groups { get; set; }
    
    [SwaggerSchema(Description = "list of teachers")]
    public virtual ICollection<Teacher> Teachers { get; set; }
}
