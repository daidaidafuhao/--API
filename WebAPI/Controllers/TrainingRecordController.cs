using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingRecordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainingRecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TrainingRecord
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingRecord>>> GetTrainingRecords()
        {
            return await _context.TrainingRecords
                .Include(t => t.Employee)
                .ToListAsync();
        }

        // GET: api/TrainingRecord/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingRecord>> GetTrainingRecord(int id)
        {
            var trainingRecord = await _context.TrainingRecords
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainingRecord == null)
            {
                return NotFound();
            }

            return trainingRecord;
        }

        // GET: api/TrainingRecord/employee/{employeeId}
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<TrainingRecord>>> GetTrainingRecordsByEmployee(string employeeId)
        {
            return await _context.TrainingRecords
                .Include(t => t.Employee)
                .Where(t => t.EmployeeId == employeeId)
                .ToListAsync();
        }

        // POST: api/TrainingRecord
        [HttpPost]
        public async Task<ActionResult<TrainingRecord>> CreateTrainingRecord(TrainingRecord trainingRecord)
        {
            _context.TrainingRecords.Add(trainingRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrainingRecord), new { id = trainingRecord.Id }, trainingRecord);
        }

        // PUT: api/TrainingRecord/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainingRecord(int id, TrainingRecord trainingRecord)
        {
            if (id != trainingRecord.Id)
            {
                return BadRequest();
            }

            _context.Entry(trainingRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingRecordExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/TrainingRecord/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingRecord(int id)
        {
            var trainingRecord = await _context.TrainingRecords.FindAsync(id);
            if (trainingRecord == null)
            {
                return NotFound();
            }

            _context.TrainingRecords.Remove(trainingRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingRecordExists(int id)
        {
            return _context.TrainingRecords.Any(e => e.Id == id);
        }
    }
}