using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about study day")]
public partial class StudyDay
{
    [SwaggerSchema(Description = "date of study day")]
    public DateOnly Date { get; set; }
    
    [SwaggerSchema(Description = "list of lessons in study day")]
    public virtual ICollection<Schedule> Lessons { get; set; }
}