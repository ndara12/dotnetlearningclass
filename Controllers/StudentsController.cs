using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetlearningclass.Entities;
using System;
using System.Threading.Tasks;

namespace dotnetlearningclass.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly LearningClassDbContext _context;

        public StudentsController(LearningClassDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Students student)
        {
            try
            {
                // Check if the incoming student data is valid
                if (student == null)
                {
                    return BadRequest("Invalid student data");
                }

                // Add the student to the database
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                // Return a 201 Created response with the newly created student
                return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as database errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Implement other CRUD operations (GET, PUT, DELETE) as needed.

        [HttpGet("{id}")]
        public async Task<ActionResult<Students>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
    }
}
