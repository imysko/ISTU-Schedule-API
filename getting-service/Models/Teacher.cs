using Newtonsoft.Json;

namespace getting_service.Models;

public record Teacher(int Id, string Fullname, string Shortname)
{
    [JsonProperty("teacher_id")]
    public int Id = Id;
    [JsonProperty("fullname")]
    public string Fullname = Fullname;
    [JsonProperty("shortname")]
    public string Shortname = Shortname;
};