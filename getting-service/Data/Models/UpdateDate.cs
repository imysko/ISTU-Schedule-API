using getting_service.Utils;
using Newtonsoft.Json;

namespace getting_service.Data.Models;

public class UpdateDate
{
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Institutes { get; set; }
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Groups { get; set; }
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Teachers { get; set; }
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly LessonsNames { get; set; }
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Classrooms { get; set; }
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly ScheduleTwoWeeks { get; set; }
    
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly ScheduleHalfYear { get; set; }
}