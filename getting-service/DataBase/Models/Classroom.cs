using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class Classroom
{
    [JsonProperty("classroom_id")]
    public int ClassroomId { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}
