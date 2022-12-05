using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class Institute
{
    [JsonProperty("institute_id")]
    public int InstituteId { get; set; }

    [JsonProperty("institute_title")]
    public string? InstituteTitle { get; set; }

    public virtual ICollection<Group> Groups { get; } = new List<Group>();
}
