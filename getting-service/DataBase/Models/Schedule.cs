using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class Schedule
{
    [JsonProperty("schedule_id")]
    public int ScheduleId { get; set; }

    [JsonProperty("groups_verbose")]
    public string? GroupsVerbose { get; set; }

    [JsonProperty("teachers_verbose")]
    public string? TeachersVerbose { get; set; }
    
    public int? ClassroomId { get; set; }
    
    [JsonProperty("auditories_verbose")]
    public string? ClassroomVerbose { get; set; }
    
    public int? DisciplinesId { get; set; }
    
    [JsonProperty("discipline_verbose")]
    public string? DisciplineVerbose { get; set; }
    
    [JsonProperty("lesson_id")]
    public int? LessonId { get; set; }
    
    [JsonProperty("subgroup")]
    public int? Subgroup { get; set; }
    
    [JsonProperty("lesson_type")]
    public int? LessonType { get; set; }
    
    [JsonProperty("date")]
    public DateOnly? Date { get; set; }

    public virtual Classroom? Classroom { get; set; }

    public virtual Discipline? Discipline { get; set; }

    public virtual LessonsTime? Lesson { get; set; }
}
