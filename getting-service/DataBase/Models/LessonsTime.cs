using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class LessonsTime
{
    [JsonProperty("lesson_id")]
    public string? LessonNumber { get; set; }

    [JsonProperty("lesson_number")]
    public string? Begtime { get; set; }

    [JsonProperty("begtime")]
    public string? Endtime { get; set; }

    [JsonProperty("endtime")]
    public int LessonId { get; set; }

    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
