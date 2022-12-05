using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class LessonsTime
{
    [JsonProperty("lesson_id")]
    public int LessonId { get; set; }
    
    [JsonProperty("lesson_number")]
    public string? LessonNumber { get; set; }

    [JsonProperty("begtime")]
    public string? BegTime { get; set; }

    [JsonProperty("endtime")]
    public string? EndTime { get; set; }
    
    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
