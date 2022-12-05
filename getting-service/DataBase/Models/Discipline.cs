using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class Discipline
{
    [JsonProperty("discipline_id")]
    public int DisciplineId { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("real_title")]
    public string? RealTitle { get; set; }

    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
