using Newtonsoft.Json;

namespace getting_service.Models;

public record Group(int Id, string Name, int Course, int InstituteId)
{
    [JsonProperty("group_id")]
    public int Id = Id;
    [JsonProperty("name")]
    public string Name = Name;
    [JsonProperty("course")]
    public int Course = Course;
    [JsonProperty("institute_id")]
    public int InstituteId = InstituteId;
};