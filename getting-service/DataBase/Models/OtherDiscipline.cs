using getting_service.Data.Enums;
using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class OtherDiscipline
{
    [JsonProperty("other_discipline_id")]
    public int OtherDisciplineId { get; set; }

    [JsonProperty("discipline_title")]
    public string? DisciplineTitle { get; set; }

    [JsonProperty("is_online")]
    public bool? IsOnline { get; set; }
    
    [JsonProperty("type")]
    public OtherDisciplineType? Type { get; set; }

    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}