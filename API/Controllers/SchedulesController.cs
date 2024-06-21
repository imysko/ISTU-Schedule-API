using System.ComponentModel.DataAnnotations;
using System.Globalization;
using API.Data.Enums;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;
using NuGet.Packaging;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("schedule-api/schedules")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public SchedulesController(ScheduleContext context)
        {
            _context = context;
        }
        
        [HttpGet("lesson-types")]
        [SwaggerOperation(Summary = "Get list of lesson types")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of lesson types")]
        public IEnumerable<LessonTypeResponse> GetInstitutes()
        {
            return Enum.GetValues(typeof(LessonType)).Cast<LessonType>()
                .Select(lt => new LessonTypeResponse
                {
                    Key = Convert.ToInt32(lt),
                    Name = lt.ToString()
                });
        }

        [HttpGet("group-schedule/month")]
        [SwaggerOperation(Summary = "Get group schedule for the month")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Month is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<StudyDay>>> GetMonthGroupSchedules(
            [SwaggerParameter(Description = "Group Id")][Required][FromQuery]int groupId,
            [SwaggerParameter(Description = "Number of month")][Required][FromQuery]int month)
        {
            if (month is < 1 or > 12)
            {
                return BadRequest();
            }
            
            var schedules = await _context.Schedules
                .Include(s => s.Classroom)
                .Include(s => s.Discipline)
                .Include(s => s.OtherDiscipline)
                .Include(s => s.Query)
                .ThenInclude(q => q.AffectedSchedule)
                .Include(s => s.LessonTime)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.ScheduleTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => s.Date.Month == month)
                .Where(s => s.ScheduleGroups.Any(sg => sg.GroupId == groupId))
                .OrderBy(s => s.Date)
                .GroupBy(s => s.Date)
                .ToListAsync();

            var studDays = GroupSchedule(schedules);
            CalculateTimeBreak(studDays);
            
            return studDays.Any()? Ok(studDays) : NotFound();
        }
        
        [HttpGet("group-schedule/weekly")]
        [SwaggerOperation(Summary = "Get group weekly schedule")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<StudyDay>>> GetWeeklyGroupSchedules(
            [SwaggerParameter(Description = "Group Id")][FromQuery][Required]int groupId,
            [SwaggerParameter(Description = "Start date of week in standard ISO 8601 YYYY-MM-DD")]
            [FromQuery][Required]string startDateWeekString)
        {
            if (!DateOnly.TryParseExact(startDateWeekString, "yyyy-MM-dd", out var startDateWeek))
            {
                return BadRequest();   
            }
            
            var schedules = await _context.Schedules
             .Include(s => s.Classroom)
             .Include(s => s.Discipline)
             .Include(s => s.OtherDiscipline)
             .Include(s => s.Query)
             .ThenInclude(q => q.AffectedSchedule)
             .Include(s => s.LessonTime)
             .Include(s => s.ScheduleGroups)
             .ThenInclude(sg => sg.Group)
             .Include(s => s.ScheduleTeachers)
             .ThenInclude(st => st.Teacher)
             .Where(s => s.Date >= startDateWeek && s.Date <= startDateWeek.AddDays(6))
             .Where(s => s.ScheduleGroups.Any(sg => sg.GroupId == groupId))
             .OrderBy(s => s.Date)
             .GroupBy(s => s.Date)
             .ToListAsync();

            var studDays = GroupSchedule(schedules);
            CalculateTimeBreak(studDays);
            
            return studDays.Any()? Ok(studDays) : NotFound();
        }
        
        [HttpGet("group-schedule/day")]
        [SwaggerOperation(Summary = "Get group daily schedule")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<StudyDay>>> GetDayGroupSchedules(
            [SwaggerParameter(Description = "Group Id")][Required][FromQuery]int groupId,
            [SwaggerParameter(Description = "Date in standard ISO 8601 YYYY-MM-DD")][Required][FromQuery]string dateString)
        {
            if (!DateOnly.TryParseExact(dateString, "yyyy-MM-dd", out var dateOnly))
            {
                return BadRequest();   
            }
            
            var schedules = await _context.Schedules
                .Include(s => s.Classroom)
                .Include(s => s.Discipline)
                .Include(s => s.OtherDiscipline)
                .Include(s => s.Query)
                .ThenInclude(q => q.AffectedSchedule)
                .Include(s => s.LessonTime)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.ScheduleTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => s.Date == dateOnly)
                .Where(s => s.ScheduleGroups.Any(sg => sg.GroupId == groupId))
                .OrderBy(s => s.Date)
                .GroupBy(s => s.Date)
                .ToListAsync();

            var studDays = GroupSchedule(schedules);
            CalculateTimeBreak(studDays);
            
            return studDays.Any()? Ok(studDays) : NotFound();
        }
        
        [HttpGet("teacher-schedule/month")]
        [SwaggerOperation(Summary = "Get teacher schedule for the month")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Month is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<StudyDay>>> GetMonthTeacherSchedules(
            [SwaggerParameter(Description = "Teacher Id")][Required][FromQuery]int teacherId,
            [SwaggerParameter(Description = "Number of month")][Required][FromQuery]int month)
        {
            if (month is < 1 or > 12)
            {
                return BadRequest();
            }
            
            var schedules = await _context.Schedules
                .Include(s => s.Classroom)
                .Include(s => s.Discipline)
                .Include(s => s.OtherDiscipline)
                .Include(s => s.Query)
                .ThenInclude(q => q.AffectedSchedule)
                .Include(s => s.LessonTime)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.ScheduleTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => s.Date.Month == month)
                .Where(s => s.ScheduleTeachers.Any(sg => sg.TeacherId == teacherId))
                .OrderBy(s => s.Date)
                .GroupBy(s => s.Date)
                .ToListAsync();
            
            var studDays = GroupSchedule(schedules);
            CalculateTimeBreak(studDays);
            
            return studDays.Any()? Ok(studDays) : NotFound();
        }
        
        [HttpGet("teacher-schedule/weekly")]
        [SwaggerOperation(Summary = "Get teacher weekly schedule")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<StudyDay>>> GetWeeklyTeacherSchedules(
            [SwaggerParameter(Description = "Teacher Id")][FromQuery][Required]int teacherId,
            [SwaggerParameter(Description = "Start date of week in standard ISO 8601 YYYY-MM-DD")]
            [FromQuery][Required]string startDateWeekString)
        {
            if (!DateOnly.TryParseExact(startDateWeekString, "yyyy-MM-dd", out var startDateWeek))
            {
                return BadRequest();   
            }
            
            var schedules = await _context.Schedules
             .Include(s => s.Classroom)
             .Include(s => s.Discipline)
             .Include(s => s.OtherDiscipline)
             .Include(s => s.Query)
             .ThenInclude(q => q.AffectedSchedule)
             .Include(s => s.LessonTime)
             .Include(s => s.ScheduleGroups)
             .ThenInclude(sg => sg.Group)
             .Include(s => s.ScheduleTeachers)
             .ThenInclude(st => st.Teacher)
             .Where(s => s.Date >= startDateWeek && s.Date <= startDateWeek.AddDays(6))
             .Where(s => s.ScheduleTeachers.Any(sg => sg.TeacherId == teacherId))
             .OrderBy(s => s.Date)
             .GroupBy(s => s.Date)
             .ToListAsync();
    
            var studDays = GroupSchedule(schedules);
            CalculateTimeBreak(studDays);
            
            return studDays.Any()? Ok(studDays) : NotFound();
        }
        
        [HttpGet("teacher-schedule/day")]
        [SwaggerOperation(Summary = "Get teacher daily schedule")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<StudyDay>>> GetDayTeacherSchedules(
            [SwaggerParameter(Description = "Teacher Id")][Required][FromQuery]int teacherId,
            [SwaggerParameter(Description = "Date in standard ISO 8601 YYYY-MM-DD")][Required][FromQuery]string dateString)
        {
            if (!DateOnly.TryParseExact(dateString, "yyyy-MM-dd", out var dateOnly))
            {
                return BadRequest();   
            }
            
            var schedules = await _context.Schedules
                .Include(s => s.Classroom)
                .Include(s => s.Discipline)
                .Include(s => s.OtherDiscipline)
                .Include(s => s.Query)
                .ThenInclude(q => q.AffectedSchedule)
                .Include(s => s.LessonTime)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.ScheduleTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => s.Date == dateOnly)
                .Where(s => s.ScheduleTeachers.Any(sg => sg.TeacherId == teacherId))
                .OrderBy(s => s.Date)
                .GroupBy(s => s.Date)
                .ToListAsync();
            
            var studDays = GroupSchedule(schedules);
            CalculateTimeBreak(studDays);
            
            return studDays.Any()? Ok(studDays) : NotFound();
        }    
        
        [HttpGet("classroom-schedule/month")]
        [SwaggerOperation(Summary = "Get classroom schedule for the month")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Month is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<StudyDay>>> GetMonthClassroomSchedules(
            [SwaggerParameter(Description = "Classroom Id")][Required][FromQuery]int classroomId,
            [SwaggerParameter(Description = "Number of month")][Required][FromQuery]int month)
        {
            if (month is < 1 or > 12)
            {
                return BadRequest();
            }
            
            var schedules = await _context.Schedules
                .Include(s => s.Classroom)
                .Include(s => s.Discipline)
                .Include(s => s.OtherDiscipline)
                .Include(s => s.Query)
                .ThenInclude(q => q.AffectedSchedule)
                .Include(s => s.LessonTime)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.ScheduleTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => s.Date.Month == month)
                .Where(s => s.ClassroomId != null && s.ClassroomId == classroomId)
                .OrderBy(s => s.Date)
                .GroupBy(s => s.Date)
                .ToListAsync();
            
            var studDays = GroupSchedule(schedules);
            CalculateTimeBreak(studDays);
            
            return studDays.Any()? Ok(studDays) : NotFound();
        }
        
        [HttpGet("classroom-schedule/weekly")]
        [SwaggerOperation(Summary = "Get classroom weekly schedule")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<StudyDay>>> GetWeeklyClassroomSchedules(
            [SwaggerParameter(Description = "Classroom Id")][FromQuery][Required]int classroomId,
            [SwaggerParameter(Description = "Start date of week in standard ISO 8601 YYYY-MM-DD")]
            [FromQuery][Required]string startDateWeekString)
        {
            if (!DateOnly.TryParseExact(startDateWeekString, "yyyy-MM-dd", out var startDateWeek))
            {
                return BadRequest();   
            }
            
            var schedules = await _context.Schedules
             .Include(s => s.Classroom)
             .Include(s => s.Discipline)
             .Include(s => s.OtherDiscipline)
             .Include(s => s.Query)
             .ThenInclude(q => q.AffectedSchedule)
             .Include(s => s.LessonTime)
             .Include(s => s.ScheduleGroups)
             .ThenInclude(sg => sg.Group)
             .Include(s => s.ScheduleTeachers)
             .ThenInclude(st => st.Teacher)
             .Where(s => s.Date >= startDateWeek && s.Date <= startDateWeek.AddDays(6))
             .Where(s => s.ClassroomId != null && s.ClassroomId == classroomId)
             .OrderBy(s => s.Date)
             .GroupBy(s => s.Date)
             .ToListAsync();
    
            var studDays = GroupSchedule(schedules);
            CalculateTimeBreak(studDays);
            
            return studDays.Any()? Ok(studDays) : NotFound();
        }
        
        [HttpGet("classroom-schedule/day")]
        [SwaggerOperation(Summary = "Get classroom daily schedule")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<StudyDay>>> GetDayClassroomSchedules(
            [SwaggerParameter(Description = "Classroom Id")][Required][FromQuery]int classroomId,
            [SwaggerParameter(Description = "Date in standard ISO 8601 YYYY-MM-DD")][Required][FromQuery]string dateString)
        {
            if (!DateOnly.TryParseExact(dateString, "yyyy-MM-dd", out var dateOnly))
            {
                return BadRequest();   
            }
            
            var schedules = await _context.Schedules
                .Include(s => s.Classroom)
                .Include(s => s.Discipline)
                .Include(s => s.OtherDiscipline)
                .Include(s => s.Query)
                .ThenInclude(q => q.AffectedSchedule)
                .Include(s => s.LessonTime)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.ScheduleTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => s.Date == dateOnly)
                .Where(s => s.ClassroomId != null && s.ClassroomId == classroomId)
                .OrderBy(s => s.Date)
                .GroupBy(s => s.Date)
                .ToListAsync();
            
            var studDays = GroupSchedule(schedules);
            CalculateTimeBreak(studDays);
            
            return studDays.Any()? Ok(studDays) : NotFound();
        }
        
        private List<StudyDay> GroupSchedule(IEnumerable<IGrouping<DateOnly, Schedule>> schedules)
        {
            return schedules
                .Select(d => new StudyDay
                {
                    Date = d.Key,
                    Lessons = d
                        .OrderBy(s => s.LessonId)
                        .GroupBy(s => s.LessonTime)
                        .Select(l => new Lesson
                        {
                            Time = l.Key!,
                            Schedules = l
                                .Where(s => s.Query is not 
                                    { Type: QueryType.Move or QueryType.MoveFrom or QueryType.MoveTo }
                                )
                                .OrderBy(s => s.Subgroup)
                                .ToList(),
                            Changes = l
                                .Where(s => 
                                    s.Query is { Type: QueryType.Move or QueryType.MoveFrom or QueryType.MoveTo }
                                )
                                .Select(async s =>
                                {
                                    if (s.Query is not { RelatedQueriesId: not null }) return s;
                                    
                                    var queryId = s.Query.RelatedQueriesId.First(id => id != s.Query.QueryId); 
                                    s.Query.ReplacedSchedule = await _context.Schedules.FirstAsync(sh => sh.QueryId == queryId);
                                    return s;
                                })
                                .Select(t => t.Result)
                                .OrderBy(s => s.Subgroup)
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();
        }
        
        private static void CalculateTimeBreak(List<StudyDay> studDays)
        {
            studDays.ForEach(d => d.Lessons
                .Select((value, i) => new
                {
                    Value = value,
                    Index = i
                })
                .ToList()
                .ForEach(obj =>
                {
                    var endTime = TimeOnly.ParseExact(obj.Value.Time.Endtime!, "H:mm", CultureInfo.InvariantCulture);
                    var begTime = endTime;
                    var lesson = d.Lessons.ElementAtOrDefault(obj.Index + 1);
                    if (lesson != null)
                    {
                        begTime = TimeOnly.ParseExact(lesson.Time.Begtime!, "H:mm", CultureInfo.InvariantCulture);
                    }

                    var breakTime = begTime - endTime;
                    obj.Value.BreakTimeAfter = breakTime.TotalMinutes > 15 ? breakTime : null;
                })
            );
        }
    }
}
