using Newtonsoft.Json;

namespace getting_service.Models;

public record Classroom(int Id, string Name)
{
    [JsonProperty("classroom_id")]
    public int Id = Id;
    [JsonProperty("name")]
    public string Name = Name;
}