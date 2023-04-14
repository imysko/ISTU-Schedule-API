using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Data.Models;

[SwaggerSchema(Description = "Information about lesson type")]
public class LessonTypeResponse
{
    public int Key { get; set; }
    
    public string Name { get; set; }
}