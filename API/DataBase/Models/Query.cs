using System.ComponentModel.DataAnnotations.Schema;
using API.Data.Enums;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DataBase.Models;

[SwaggerSchema(Description = "Information about query")]
public class Query
{
    [SwaggerSchema(Description = "query Id")]
    public int QueryId { get; set; }

    [SwaggerSchema(Description = "description of the query")]
    public string? Description { get; set; }
    
    [SwaggerSchema(Description = "date of the query")]
    public DateOnly Date { get; set; }

    [SwaggerSchema(Description = "type of the query")]
    public QueryType? Type { get; set; }

    [SwaggerSchema(Description = "affected schedule id")]
    public int? AffectedScheduleId { get; set; }
    
    [SwaggerSchema(Description = "related queries")]
    public int[]? RelatedQueriesId { get; set; }
    
    [SwaggerSchema(Description = "affected schedule")]
    public virtual Schedule? AffectedSchedule { get; set; }

    [JsonIgnore]
    [SwaggerSchema(WriteOnly = true)]
    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();

    [NotMapped]
    public virtual Schedule? ReplacedSchedule { get; set; }
}