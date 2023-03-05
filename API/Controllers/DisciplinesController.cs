using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("schedule-api/disciplines")]
    [ApiController]
    public class DisciplinesController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public DisciplinesController(ScheduleContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get list of disciplines")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of disciplines")]
        public async Task<ActionResult<IEnumerable<Discipline>>> GetDisciplines()
        {
            return await _context.Disciplines
                .OrderBy(d => d.Title)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get discipline by id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received discipline")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Discipline not found")]
        public async Task<ActionResult<Discipline>> GetDiscipline(
            [SwaggerParameter(Description = "Discipline id")]int id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);

            if (discipline == null)
            {
                return NotFound();
            }

            return discipline;
        }
    }
}
