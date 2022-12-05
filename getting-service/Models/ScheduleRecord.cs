using Newtonsoft.Json;

namespace getting_service.Models;

public record ScheduleRecord(int Id, string GroupsVerbose, string TeachersVerbose, string ClassroomsVerbose, string DisciplineVerbose, int LessonId, int Subgroup, int LessonType, string LessonDate)
{
    [JsonProperty("schedule_id")]
    public int Id = Id;
    [JsonProperty("groups_verbose")]
    public string GroupsVerbose = GroupsVerbose;
    [JsonProperty("teachers_verbose")]
    public string TeachersVerbose = TeachersVerbose;
    [JsonProperty("auditories_verbose")]
    public string ClassroomsVerbose = ClassroomsVerbose;
    [JsonProperty("discipline_verbose")]
    public string DisciplineVerbose = DisciplineVerbose;
    [JsonProperty("lesson_id")]
    public int LessonId = LessonId;
    [JsonProperty("subgroup")]
    public int Subgroup = Subgroup;
    [JsonProperty("lesson_type")]
    public int LessonType = LessonType;
    [JsonProperty("date")]
    public string LessonDate = LessonDate;
}