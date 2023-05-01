using getting_service.Data.Enums;
using getting_service.DataBase.Models;
using Newtonsoft.Json;

namespace getting_service.Data.Models;

public partial class OtherDisciplineResponse
{
    [JsonProperty("other_discipline_id")]
    public int OtherDisciplineId { get; set; }

    [JsonProperty("discipline_title")]
    public string? DisciplineTitle { get; set; }

    [JsonProperty("is_online")]
    public bool? IsOnline { get; set; }
    
    [JsonProperty("type")]
    public OtherDisciplineType? Type { get; set; }
    
    [JsonProperty("is_active")]
    public bool? IsActive { get; set; }
    
    [JsonProperty("project_active")]
    public bool? ProjectActive { get; set; }
}