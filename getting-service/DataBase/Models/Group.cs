using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class Group
{
    [JsonProperty("group_id")]
    public int GroupId { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("course")]
    public int? Course { get; set; }

    [JsonProperty("institute_id")]
    public int? InstituteId { get; set; }

    public virtual Institute? Institute { get; set; }
    
    public virtual ICollection<ScheduleGroup> ScheduleGroups { get; set; }
}
