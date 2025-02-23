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
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees([FromQuery] string searchTerm)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.Name.Contains(searchTerm) || e.IDCardNumber.Contains(searchTerm));
            }

            return await query.ToListAsync();
        }

        // GET: api/Employee/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployees([FromQuery] Employee filter)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(e => e.Name.Contains(filter.Name));

            if (!string.IsNullOrEmpty(filter.IDCardNumber))
                query = query.Where(e => e.IDCardNumber.Contains(filter.IDCardNumber));

            if (!string.IsNullOrEmpty(filter.Education))
                query = query.Where(e => e.Education.Contains(filter.Education));

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(e => e.Title.Contains(filter.Title));

            if (!string.IsNullOrEmpty(filter.Level))
                query = query.Where(e => e.Level.Contains(filter.Level));

            if (!string.IsNullOrEmpty(filter.LevelJobType))
                query = query.Where(e => e.LevelJobType.Contains(filter.LevelJobType));

            if (!string.IsNullOrEmpty(filter.Position))
                query = query.Where(e => e.Position.Contains(filter.Position));

            if (!string.IsNullOrEmpty(filter.UnitName))
                query = query.Where(e => e.UnitName.Contains(filter.UnitName));

            if (filter.RuzhiDateStart != DateTime.MinValue && filter.RuzhiDateEnd != DateTime.MinValue)
                query = query.Where(e => e.RuzhiDate >= filter.RuzhiDateStart && e.RuzhiDate <= filter.RuzhiDateEnd);

            if (!string.IsNullOrEmpty(filter.SchoolName))
                query = query.Where(e => e.SchoolName.Contains(filter.SchoolName));

            if (!string.IsNullOrEmpty(filter.ZhuanYe))
                query = query.Where(e => e.ZhuanYe.Contains(filter.ZhuanYe));

            return await query.ToListAsync();
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<ActionResult<Employee>> InsertEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee);
        }

        // PUT: api/Employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}