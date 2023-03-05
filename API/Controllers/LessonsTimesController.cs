using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("schedule-api/lessons-times")]
    [ApiController]
    public class LessonsTimesController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public LessonsTimesController(ScheduleContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get list of lessons times")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of lessons times")]
        public async Task<ActionResult<IEnumerable<LessonsTime>>> GetLessonsTimes()
        {
            return await _context.LessonsTimes
                .OrderBy(lt => lt.LessonId)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get lesson time by id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received lesson time")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Lesson time not found")]
        public async Task<ActionResult<LessonsTime>> GetLessonsTime(
            [SwaggerParameter(Description = "Lesson time id")]int id)
        {
            var lessonsTime = await _context.LessonsTimes.FindAsync(id);

            if (lessonsTime == null)
            {
                return NotFound();
            }

            return lessonsTime;
        }
    }
}
