using API.DataBase.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Data.Models;

[SwaggerSchema(Description = "Information about course")]
public partial class Course
{
    [SwaggerSchema(Description = "course number")]
    public int? CourseNumber { get; set; }
    
    [SwaggerSchema(Description = "list of groups this course")]
    public virtual ICollection<Group> Groups { get; set; }
}