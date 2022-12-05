using Newtonsoft.Json;

namespace getting_service.Models;

public record Discipline(int Id, string Title, string RealTitle)
{
    [JsonProperty("discipline_id")]
    public int Id = Id;
    [JsonProperty("title")]
    public string Title = Title;
    [JsonProperty("real_title")]
    public string RealTitle = RealTitle;
};