using getting_service.Data.Enums;

namespace getting_service.DataBase.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public string? GroupsVerbose { get; set; }

    public string? TeachersVerbose { get; set; }
    
    public int? ClassroomId { get; set; }

    public string? ClassroomVerbose { get; set; }

    public int? DisciplineId { get; set; }
    
    public string? DisciplineVerbose { get; set; }
    
    public int? OtherDisciplineId { get; set; }
    
    public int? LessonId { get; set; }
    
    public Subgroup? Subgroup { get; set; }
    
    public LessonType? LessonType { get; set; }
    
    public string? ScheduleType { get; set; }
    
    public DateOnly? Date { get; set; }

    public virtual Classroom? Classroom { get; set; }
    
    public virtual Discipline? Discipline { get; set; }

    public virtual OtherDiscipline? OtherDiscipline { get; set; }

    public virtual LessonsTime? LessonTime { get; set; }
    
    public virtual ICollection<ScheduleGroup> ScheduleGroups { get; set; }
    
    public virtual ICollection<ScheduleTeacher> ScheduleTeachers { get; set; }
}
