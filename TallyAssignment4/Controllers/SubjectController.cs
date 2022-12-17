using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallyAssignment4.Data;
using TallyAssignment4.Models;

namespace TallyAssignment4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly StudentDBContext _dbContext;
        public SubjectController(StudentDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        //Getting all the subjects present in DB
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            IEnumerable<Subject> subjects = await _dbContext.Subjects.ToListAsync();
            return Ok(subjects);
        }

        //Getting subject with given SubId
        [HttpGet("{subId}")]
        public async Task<ActionResult<Subject>> GetSubjeect(int subId)
        {
            var subject = await _dbContext.Subjects.FirstOrDefaultAsync(sub => sub.SubId == subId);
            if(subject == null)
            {
                return NotFound();
            }
            return Ok(subject);
        }

        //Adding new subject to any student with given StudId
        [HttpPost]
        public async Task<ActionResult<Subject>> AddSubject(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (_dbContext.Students.FirstOrDefault(s => s.StudId == subject.StudentStudId) == null)
            {
                ModelState.AddModelError("CustomError", "Student with given Id is not present");
                return BadRequest(ModelState);
            }
            await _dbContext.Subjects.AddAsync(subject);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction("GetSubjects", new { id = subject.SubId }, subject);
        }

        //Updating subject based on SubId
        [HttpPut("{subId}")]
        public async Task<ActionResult<Subject>> UpdateSubject(int subId, Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (subId != subject.SubId)
            {
                return BadRequest();
            }
            var sub = _dbContext.Subjects.AsNoTracking().FirstOrDefault(s => s.SubId == subject.SubId && s.StudentStudId == subject.StudentStudId);
            if (sub == null)
            {
                ModelState.AddModelError("CustomError", "The subject matching with given studid and subid is not present");
                return BadRequest(ModelState);
            }
            _dbContext.Entry(subject).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(subject);
        }

        //[HttpDelete("{subId}/{studId}")]
        //Deleting subject with SubId
        [HttpDelete("{subId}")]
        public async Task<ActionResult<Student>> DeleteSubject(int subId)
        {
            var sub = await _dbContext.Subjects.FirstOrDefaultAsync(s => s.SubId == subId);
            if (sub == null)
            {
                ModelState.AddModelError("CustomError", "The subject matching with given studid and subid is not present");
                return BadRequest(ModelState);
            }
            _dbContext.Subjects.Remove(sub);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
