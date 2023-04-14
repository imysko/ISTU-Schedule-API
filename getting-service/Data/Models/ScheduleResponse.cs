using getting_service.Data.Enums;
using Newtonsoft.Json;

namespace getting_service.Data.Models;

public class ScheduleResponse
{
    [JsonProperty("schedule_id")]
    public int ScheduleId { get; set; }
    
    [JsonProperty("groups_ids")]
    public ICollection<int?>? GroupsIds { get; set; }

    [JsonProperty("groups_verbose")]
    public string? GroupsVerbose { get; set; }

    [JsonProperty("teachers_ids")]
    public ICollection<int?>? TeachersIds { get; set; }
    
    [JsonProperty("teachers_verbose")]
    public string? TeachersVerbose { get; set; }
    
    [JsonProperty("auditories_ids")]
    public ICollection<int?>? ClassroomsIds { get; set; }
    
    [JsonProperty("auditories_verbose")]
    public string? ClassroomsVerbose { get; set; }
    
    [JsonProperty("discipline_id")]
    public int? DisciplineId { get; set; }
    
    [JsonProperty("discipline_verbose")]
    public string? DisciplineVerbose { get; set; }
    
    [JsonProperty("other_discipline")]
    public int? OtherDisciplineId { get; set; }
    
    [JsonProperty("lesson_id")]
    public int? LessonId { get; set; }
    
    [JsonProperty("subgroup")]
    public Subgroup? Subgroup { get; set; }
    
    [JsonProperty("lesson_type")]
    public LessonType? LessonType { get; set; }

    [JsonProperty("schedule_type")]
    public string? ScheduleType { get; set; }

    [JsonProperty("date")]
    public string Date { get; set; }
}