using API.DataBase.Context;
using API.DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("schedule-api/other-disciplines")]
    [ApiController]
    public class OtherDisciplinesController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public OtherDisciplinesController(ScheduleContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get list of other disciplines")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of other disciplines")]
        public async Task<ActionResult<IEnumerable<OtherDiscipline>>> GetOtherDisciplines()
        {
            return await _context.OtherDisciplines
                .Where(d => d.IsActive == true && d.ProjectActive == true)
                .OrderBy(d => d.DisciplineTitle)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get other discipline by id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received other discipline")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Other discipline not found")]
        public async Task<ActionResult<OtherDiscipline>> GetOtherDiscipline(
            [SwaggerParameter(Description = "Other discipline id")]int id)
        {
            var otherDiscipline = await _context.OtherDisciplines.FindAsync(id);

            if (otherDiscipline == null)
            {
                return NotFound();
            }

            return otherDiscipline;
        }
    }
}