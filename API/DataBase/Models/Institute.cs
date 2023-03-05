using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about institute")]
public partial class Institute
{
    [SwaggerSchema(Description = "institute Id")]
    public int InstituteId { get; set; }

    [SwaggerSchema(Description = "title of the institute")]
    public string? InstituteTitle { get; set; }

    [SwaggerSchema(Description = "list of groups")]
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
