using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallyAssignment4.Data;
using TallyAssignment4.Models;

namespace TallyAssignment4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDBContext _dbContext;
        public StudentController(StudentDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        //Returning Studednt with related subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _dbContext.Students
                                        .Include(sub => sub.Subjects)
                                        .ToListAsync();
        }

        //Returning specific student with related subject bsed on id
        [HttpGet("GetStudent/{studId}")]
        public async Task<ActionResult<Student>> GetStudent(int studId)
        {
            var student = await _dbContext.Students
                                       .Include(stu => stu.Subjects)
                                       .Where(stu => stu.StudId == studId)
                                       .FirstOrDefaultAsync();
            if(student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        //Adding new student with or without one or more subjects
        [HttpPost]
        public async Task<ActionResult<Student>> AddStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _dbContext.Students.AddAsync(student);
            await _dbContext.Subjects.AddRangeAsync(student.Subjects);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction("GetStudents", new { id = student.StudId }, student);
        }


        [HttpPut("{studId}")]
        public async Task<ActionResult<Student>> UpdateStudent(int studId, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (studId != student.StudId)
            {
                return BadRequest();
            }
            var stud = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.StudId == student.StudId);
            if (stud == null)
            {
                ModelState.AddModelError("CustomError", "Student with given Id is not present");
                return BadRequest(ModelState);
            }
            _dbContext.Entry(student).State = EntityState.Modified;
            //List<Subject> subjects = await _dbContext.Subjects.Where(sub => sub.StudentStudId == studId).ToListAsync();
            int subCount = student.Subjects.Count();
            if(subCount > 0)
            {
                _dbContext.Subjects.UpdateRange(student.Subjects);
            }
            //_dbContext.Entry(student.Subjects).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(student);
        }

        [HttpDelete("{studId}")]
        public async Task<IActionResult> DeleteStudent(int studId)
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.StudId == studId);
            if (student == null)
            {
                ModelState.AddModelError("CustomError", "Student with given id is not present");
                return BadRequest(ModelState);
            }
            List<Subject> subjects = await _dbContext.Subjects.Where(sub => sub.StudentStudId == studId).ToListAsync();
            _dbContext.Subjects.RemoveRange(subjects);
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
