using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
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
        public async Task<ActionResult<IEnumerable<InstituteResponse>>> GetInstitutes()
        {
            return await _context.Institutes
                .Include(i => i.Groups
                    .OrderBy(g => g.Course))
                .OrderBy(i => i.InstituteTitle)
                .Select(i => new InstituteResponse
                {
                    InstituteId = i.InstituteId,
                    InstituteTitle = i.InstituteTitle,
                    Courses = i.Groups
                        .GroupBy(g => g.Course)
                        .Select(c => new Course
                        {
                            CourseNumber = c.Key,
                            Groups = c
                                .OrderBy(g => g.Name)
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get institute by id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received institute")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Institute not found")]
        public async Task<ActionResult<InstituteResponse>> GetInstitute(
            [SwaggerParameter(Description = "Institute id")]int id)
        {
            var institutes = await _context.Institutes
                .Include(i => i.Groups
                    .OrderBy(g => g.Course))
                .OrderBy(i => i.InstituteTitle)
                .Select(i => new InstituteResponse
                {
                    InstituteId = i.InstituteId,
                    InstituteTitle = i.InstituteTitle,
                    Courses = i.Groups
                        .GroupBy(g => g.Course)
                        .Select(c => new Course
                        {
                            CourseNumber = c.Key,
                            Groups = c
                                .OrderBy(g => g.Name)
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            var institute = institutes.FirstOrDefault(i => i.InstituteId == id);

            return institute == null ? NotFound() : institute;
        }
    }
}
