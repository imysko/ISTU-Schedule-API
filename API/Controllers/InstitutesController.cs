using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("schedule-api/institutes")]
    [ApiController]
    public class InstitutesController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public InstitutesController(ScheduleContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get list of institutes")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of institutes")]
        public async Task<ActionResult<IEnumerable<Institute>>> GetInstitutes()
        {
            return await _context.Institutes
                .Include(i => i.Groups
                    .OrderBy(g => g.Course)
                    .ThenBy(g => g.Name))
                .OrderBy(i => i.InstituteTitle)
                .ToListAsync();
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get institute by id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received institute")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Institute not found")]
        public async Task<ActionResult<Institute>> GetInstitute(
            [SwaggerParameter(Description = "Institute id")]int id)
        {
            var institute = await _context.Institutes
                .Include(i => i.Groups)
                .FirstOrDefaultAsync(i => i.InstituteId == id);
            
            return institute == null ? NotFound() : institute;
        }
    }
}
