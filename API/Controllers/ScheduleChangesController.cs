using System.ComponentModel.DataAnnotations;
using API.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[Produces("application/json")]
[Route("schedule-api/changes")]
[ApiController]
public class ScheduleChangesController : ControllerBase
{
    private readonly ScheduleContext _context;
    
    public ScheduleChangesController(ScheduleContext context)
    {
        _context = context;
    }
    
    [HttpGet("group-schedule")]
    [SwaggerOperation(Summary = "Get changes for group")]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of changes")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
    [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Changes not found")]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetGroupChanges(
        [SwaggerParameter(Description = "Group Id")][Required][FromQuery]int groupId,
        [SwaggerParameter(Description = "Start date of week in standard ISO 8601 YYYY-MM-DD")]
        [FromQuery][Required]string dateFromString)
    {
        if (!DateOnly.TryParseExact(dateFromString, "yyyy-MM-dd", out var dateFrom))
        {
            return BadRequest();   
        }
        
        var changes = await _context.Schedules
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
            .Where(s => s.ScheduleGroups.Any(sg => sg.GroupId == groupId))
            .Where(s => s.Query != null && (
                s.Query.Type == QueryType.Move || s.Query.Type == QueryType.MoveTo || s.Query.Type == QueryType.MoveFrom))
            .Where(s => s.Date >= dateFrom)
            .OrderByDescending(c => c.Date)
            .ToListAsync();
        
        changes = changes.Select(async s =>
        {
            if (s.Query is not { RelatedQueriesId: not null }) return s;
                                    
            var queryId = s.Query.RelatedQueriesId.First(id => id != s.Query.QueryId); 
            s.Query.ReplacedSchedule = await _context.Schedules
                .Include(sh => sh.LessonTime)
                .Include(sh => sh.Discipline)
                .Include(sh => sh.Classroom)
                .FirstAsync(sh => sh.QueryId == queryId);

            return s;
        })
        .Select(l => l.Result)
        .ToList();

        return changes.Any() ? Ok(changes) : NotFound();
    }
    
    [HttpGet("teacher-schedule")]
    [SwaggerOperation(Summary = "Get changes for teacher")]
    [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of changes")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Date is incorrect")]
    [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Changes not found")]
    public async Task<ActionResult<IEnumerable<Query>>> GetTeacherChanges(
        [SwaggerParameter(Description = "Teacher Id")][Required][FromQuery]int teacherId,
        [SwaggerParameter(Description = "Start date of week in standard ISO 8601 YYYY-MM-DD")]
        [FromQuery][Required]string dateFromString)
    {
        if (!DateOnly.TryParseExact(dateFromString, "yyyy-MM-dd", out var dateFrom))
        {
            return BadRequest();   
        }
        
        var changes = await _context.Schedules
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
            .Where(s => s.ScheduleTeachers.Any(st => st.TeacherId == teacherId))
            .Where(s => s.Query != null && (
                s.Query.Type == QueryType.Move || s.Query.Type == QueryType.MoveTo || s.Query.Type == QueryType.MoveFrom))
            .Where(s => s.Date >= dateFrom)
            .OrderByDescending(c => c.Date)
            .ToListAsync();
        
        changes = changes.Select(async s =>
            {
                if (s.Query is not { RelatedQueriesId: not null }) return s;
                                    
                var queryId = s.Query.RelatedQueriesId.First(id => id != s.Query.QueryId); 
                s.Query.ReplacedSchedule = await _context.Schedules
                    .Include(sh => sh.LessonTime)
                    .Include(sh => sh.Discipline)
                    .Include(sh => sh.Classroom)
                    .FirstAsync(sh => sh.QueryId == queryId);

                return s;
            })
            .Select(l => l.Result)
            .ToList();

        return changes.Any() ? Ok(changes) : NotFound();
    }
}