using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about institute")]
public partial class InstituteResponse
{
    [SwaggerSchema(Description = "institute Id")]
    public int InstituteId { get; set; }

    [SwaggerSchema(Description = "title of the institute")]
    public string? InstituteTitle { get; set; }

    [SwaggerSchema(Description = "list of course")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
