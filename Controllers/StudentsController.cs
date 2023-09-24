using Microsoft.AspNetCore.Mvc;
using dotnetlearningclass.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace dotnetlearningclass.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IRepository<Students> _repository;

        public StudentsController(IRepository<Students> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all students", Description = "Retrieves a list of all students.")]
        public async Task<ActionResult<IEnumerable<Students>>> GetAllStudents()
        {
            var students = await _repository.GetAllAsync();
            return Ok(students);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new student", Description = "Creates a new student and adds it to the repository.")]
        public async Task<IActionResult> CreateStudent(Students student)
        {
            try
            {
                // Check if the incoming student data is valid
                if (student == null)
                {
                    return BadRequest("Invalid student data");
                }

                // Add the student to the repository
                await _repository.AddAsync(student);

                // Return a 201 Created response with the newly created student
                return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as repository errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

       

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a student by ID", Description = "Retrieves a student by their ID.")]
        public async Task<ActionResult<Students>> GetStudent(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
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
            var existingStudent = await _repository.GetByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound(); // Return NotFoundResult instead of NotFoundObjectResult
            }

            // Update the student's information
            existingStudent.StudentName = student.StudentName;
            existingStudent.StudentClassId = student.StudentClassId;

            try
            {
                await _repository.UpdateAsync(existingStudent);
                return NoContent(); // Return a 204 No Content response
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as repository errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a student by ID", Description = "Deletes a student by their ID.")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            // Check if the student with the given ID exists
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound("Student not found");
            }

            // Remove the student from the repository
            await _repository.DeleteAsync(student);

            return NoContent(); // Return a 204 No Content response
        }
    }
}
