using Newtonsoft.Json;

namespace getting_service.Models;

public record LessonsTime(int Id, string Number, string BegTime, string EndTime)
{
    [JsonProperty("lesson_id")]
    public int Id = Id;

    [JsonProperty("lesson_number")]
    public string Number = Number;
    
    [JsonProperty("begtime")]
    public string BegTime = BegTime;
    
    [JsonProperty("endtime")]
    public string EndTime = EndTime;
}