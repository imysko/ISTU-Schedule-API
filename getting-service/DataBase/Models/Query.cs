using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public class Query
{
    [JsonProperty("query_id")]
    public int QueryId { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}