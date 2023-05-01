using getting_service.Data.Enums;
using Newtonsoft.Json;

namespace getting_service.DataBase.Models;

public partial class OtherDiscipline
{
    public int OtherDisciplineId { get; set; }

    public string? DisciplineTitle { get; set; }

    public bool? IsOnline { get; set; }
    
    public OtherDisciplineType? Type { get; set; }
    
    public bool? IsActive { get; set; }
    
    public bool? ProjectActive { get; set; }
    
    public int? ProjfairProjectId { get; set; }

    public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
}