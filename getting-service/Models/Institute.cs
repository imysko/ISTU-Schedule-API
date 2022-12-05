using Newtonsoft.Json;

namespace getting_service.Models;

public record Institute(int Id, string Title)
{
    [JsonProperty("institute_id")]
    public int Id = Id;
    [JsonProperty("institute_title")]
    public string Title = Title;
}