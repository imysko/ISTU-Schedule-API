using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("schedule-api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public GroupsController(ScheduleContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get list of groups")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received list of groups")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups(
            [SwaggerParameter(Required = false, Description = "Institute id")]int? instituteId)
        {
            return instituteId switch
            {
                null => await _context.Groups
                    .Where(g => g.IsActive == true)
                    .OrderBy(g => g.InstituteId)
                    .ThenBy(g => g.Name)
                    .ToListAsync(),
                _ => await _context.Groups
                    .Where(g => g.IsActive == true && g.InstituteId == instituteId)
                    .OrderBy(g => g.Course)
                    .ThenBy(g => g.Name)
                    .ToListAsync()
            };
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get group by id")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Received group")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Group not found")]
        public async Task<ActionResult<Group>> GetGroup(
            [SwaggerParameter(Description = "Group id")]int id)
        {
            var group = await _context.Groups
                .Include(g => g.Institute)
                .Where(g => g.IsActive == true)
                .FirstOrDefaultAsync(g => g.GroupId == id);
            
            return group == null ? NotFound() : group;
        }
    }
}
