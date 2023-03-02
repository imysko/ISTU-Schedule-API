using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataBase.Models;

public partial class ScheduleGroup
{
    [Column("schedule_id")]
    public int ScheduleId { get; set; }
    public Schedule Schedule { get; set; }

    [Column("group_id")]
    public int GroupId { get; set; }
    public Group Group { get; set; }
}