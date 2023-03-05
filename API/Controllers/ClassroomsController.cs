using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("schedule-api/classrooms")]
    [ApiController]
    public class ClassroomsController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public ClassroomsController(ScheduleContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get list of classrooms")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of classrooms")]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetClassrooms()
        {
            return await _context.Classrooms
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get classroom by id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received classroom")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "classroom not found")]
        public async Task<ActionResult<Classroom>> GetClassroom(
            [SwaggerParameter(Description = "Classroom id")]int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);

            if (classroom == null)
            {
                return NotFound();
            }

            return classroom;
        }
    }
}
