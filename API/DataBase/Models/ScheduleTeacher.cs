using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataBase.Models;

public partial class ScheduleTeacher
{
    [Column("schedule_id")]
    public int ScheduleId { get; set; }
    public Schedule Schedule { get; set; }

    [Column("teacher_id")]
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }
}