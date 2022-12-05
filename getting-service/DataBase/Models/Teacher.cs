using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class Teacher
{
    [JsonProperty("teacher_id")]
    public int TeacherId { get; set; }

    [JsonProperty("fullname")]
    public string? Fullname { get; set; }

    [JsonProperty("shortname")]
    public string? Shortname { get; set; }
}
