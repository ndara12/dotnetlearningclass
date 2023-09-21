using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetlearningclass.Entities;
using System;
using System.Collections.Generic; // Import this namespace for IEnumerable
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;// Import this namespace for Swagger annotations

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
        [SwaggerOperation(Summary = "Create a new student", Description = "Creates a new student and adds it to the database.")]
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

        [HttpGet]
        [SwaggerOperation(Summary = "Get all students", Description = "Retrieves a list of all students.")]
        public async Task<ActionResult<IEnumerable<Students>>> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a student by ID", Description = "Retrieves a student by their ID.")]
        public async Task<ActionResult<Students>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a student by ID", Description = "Updates a student's information by their ID.")]
        public async Task<IActionResult> UpdateStudent(int id, Students student)
        {
            // Check if the incoming student data is valid
            if (student == null || id != student.StudentId)
            {
                return BadRequest("Invalid student data");
            }

            // Check if the student with the given ID exists
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound("Student not found");
            }

            // Update the student's information
            existingStudent.StudentName = student.StudentName;
            existingStudent.StudentClassId = student.StudentClassId;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // Return a 204 No Content response
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as database errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a student by ID", Description = "Deletes a student by their ID.")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            // Check if the student with the given ID exists
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound("Student not found");
            }

            // Remove the student from the database
            _context.Students.Remove(student);

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // Return a 204 No Content response
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as database errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

      
    }
}
