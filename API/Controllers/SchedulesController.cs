﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;
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
            
            return schedules.Any()? Ok(
                schedules.Select(d => new StudyDay
                {
                    Date = d.Key,
                    Lessons = d
                        .OrderBy(s => s.LessonId)
                        .ThenBy(s => s.Subgroup)
                        .ToList()   
                })
            ) : NotFound();
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
    
            return schedules.Any()? Ok(
                schedules.Select(d => new StudyDay
                    {
                        Date = d.Key,
                        Lessons = d
                            .OrderBy(s => s.LessonId)
                            .ThenBy(s => s.Subgroup)
                            .ToList()   
                    })
                ) : NotFound();
        }
        
        [HttpGet("group-schedule/day")]
        [SwaggerOperation(Summary = "Get group daily schedule")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetDayGroupSchedules(
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
                .Include(s => s.LessonTime)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.ScheduleTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => s.Date == dateOnly)
                .Where(s => s.ScheduleGroups.Any(sg => sg.GroupId == groupId))
                .OrderBy(s => s.Date)
                .ThenBy(s => s.LessonId)
                .ThenBy(s => s.Subgroup)
                .ToListAsync();
            
            return schedules.Any() ? schedules : NotFound();
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
            
            return schedules.Any()? Ok(
                schedules.Select(d => new StudyDay
                {
                    Date = d.Key,
                    Lessons = d
                        .OrderBy(s => s.LessonId)
                        .ThenBy(s => s.Subgroup)
                        .ToList()   
                })
            ) : NotFound();
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
    
            return schedules.Any()? Ok(
                schedules.Select(d => new StudyDay
                    {
                        Date = d.Key,
                        Lessons = d
                            .OrderBy(s => s.LessonId)
                            .ThenBy(s => s.Subgroup)
                            .ToList()   
                    })
                ) : NotFound();
        }
        
        [HttpGet("teacher-schedule/day")]
        [SwaggerOperation(Summary = "Get teacher daily schedule")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetDayTeacherSchedules(
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
                .Include(s => s.LessonTime)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.ScheduleTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => s.Date == dateOnly)
                .Where(s => s.ScheduleTeachers.Any(sg => sg.TeacherId == teacherId))
                .OrderBy(s => s.Date)
                .ThenBy(s => s.LessonId)
                .ThenBy(s => s.Subgroup)
                .ToListAsync();
            
            return schedules.Any() ? schedules : NotFound();
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
            
            return schedules.Any()? Ok(
                schedules.Select(d => new StudyDay
                {
                    Date = d.Key,
                    Lessons = d
                        .OrderBy(s => s.LessonId)
                        .ThenBy(s => s.Subgroup)
                        .ToList()   
                })
            ) : NotFound();
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
    
            return schedules.Any()? Ok(
                schedules.Select(d => new StudyDay
                    {
                        Date = d.Key,
                        Lessons = d
                            .OrderBy(s => s.LessonId)
                            .ThenBy(s => s.Subgroup)
                            .ToList()   
                    })
                ) : NotFound();
        }
        
        [HttpGet("classroom-schedule/day")]
        [SwaggerOperation(Summary = "Get classroom daily schedule")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of schedule")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Schedule not found")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetDayClassroomSchedules(
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
                .Include(s => s.LessonTime)
                .Include(s => s.ScheduleGroups)
                .ThenInclude(sg => sg.Group)
                .Include(s => s.ScheduleTeachers)
                .ThenInclude(st => st.Teacher)
                .Where(s => s.Date == dateOnly)
                .Where(s => s.ClassroomId != null && s.ClassroomId == classroomId)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.LessonId)
                .ThenBy(s => s.Subgroup)
                .ToListAsync();
            
            return schedules.Any() ? schedules : NotFound();
        }
    }
}
