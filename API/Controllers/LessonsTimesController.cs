using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DataBase.Context;
using API.DataBase.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsTimesController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public LessonsTimesController(ScheduleContext context)
        {
            _context = context;
        }

        // GET: api/LessonsTimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonsTime>>> GetLessonsTimes()
        {
            return await _context.LessonsTimes.ToListAsync();
        }

        // GET: api/LessonsTimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonsTime>> GetLessonsTime(int id)
        {
            var lessonsTime = await _context.LessonsTimes.FindAsync(id);

            if (lessonsTime == null)
            {
                return NotFound();
            }

            return lessonsTime;
        }

        // PUT: api/LessonsTimes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLessonsTime(int id, LessonsTime lessonsTime)
        {
            if (id != lessonsTime.LessonId)
            {
                return BadRequest();
            }

            _context.Entry(lessonsTime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonsTimeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LessonsTimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LessonsTime>> PostLessonsTime(LessonsTime lessonsTime)
        {
            _context.LessonsTimes.Add(lessonsTime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLessonsTime", new { id = lessonsTime.LessonId }, lessonsTime);
        }

        // DELETE: api/LessonsTimes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLessonsTime(int id)
        {
            var lessonsTime = await _context.LessonsTimes.FindAsync(id);
            if (lessonsTime == null)
            {
                return NotFound();
            }

            _context.LessonsTimes.Remove(lessonsTime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LessonsTimeExists(int id)
        {
            return _context.LessonsTimes.Any(e => e.LessonId == id);
        }
    }
}
