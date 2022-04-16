using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services;
using WebApi.Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Student
        [HttpGet]
        public ActionResult<IEnumerable<StudentDto>> Get()
        {
            return Ok(_studentService.GetAllStudents().Select(student => StudentDto.FromModel(student)));
        }
        // Post api/Student
        [HttpPost]
        public ActionResult Create([FromBody] StudentDto value)
        {
            var result = _studentService.CreateStudent(value.ToModel());
            if (result.HasErrors)
                return BadRequest(result.Errors);
            return Accepted();
        }

        // GET api/Student/5
        [HttpGet("{id}")]
        public ActionResult<StudentDto> Get(int id)
        {
            var student = _studentService.GetStudentById(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(StudentDto.FromModel(student));
        }
        
        // PUT api/Student/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] StudentDto value)
        {
            var result = _studentService.UpdateStudent(value.ToModel());
            if (result.HasErrors)
            {
                return BadRequest(result.Errors);
            }
            return Accepted();
        }

        // DELETE api/Student/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _studentService.DeleteStudent(id);
            return Accepted();
        }
    }
}
