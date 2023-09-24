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
        [HttpGet]
        [SwaggerOperation(Summary = "Get all students", Description = "Retrieves a list of all students.")]
        public async Task<ActionResult<IEnumerable<Students>>> GetAllStudents()
        {
            var students = await _repository.GetAllAsync();
            return Ok(students);
        }
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a student by ID", Description = "Updates a student's information by their ID.")]
        public async Task<IActionResult> UpdateStudent(int id, Students student)
        {
            // Check if the incoming student data is valid
            if (student == null)
            {
                return BadRequest("Invalid student data");
            }

            // Check if the student with the given ID exists
            var existingStudent = await _repository.GetByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound(); // Return NotFoundResult instead of NotFoundObjectResult
            }

            // Check if the StudentId in the incoming object has changed
            if (id != student.StudentId)
            {
                return BadRequest("You can't change the StudentId.");
            }

            // Update the student's information
            existingStudent.StudentName = student.StudentName;
            existingStudent.StudentClassId = student.StudentClassId;

            try
            {
                await _repository.UpdateAsync(existingStudent);

                return Ok(); 
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

            return NoContent(); 
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

                // Check if a student with the same ID already exists
                var existingStudent = await _repository.GetByIdAsync(student.StudentId);
                if (existingStudent != null)
                {
                    return Conflict("A student with the same ID already exists.");
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

    }
}
