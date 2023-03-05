using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("schedule-api/teachers")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public TeachersController(ScheduleContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get list of teachers")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of teachers")]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers
                .OrderBy(t => t.Fullname)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get teacher by id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received teacher")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Teacher not found")]
        public async Task<ActionResult<Teacher>> GetTeacher(
            [SwaggerParameter(Description = "Teacher id")]int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }
    }
}
