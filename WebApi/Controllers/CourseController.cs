using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services;
using WebApi.Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }
        // GET: api/Course
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> Get()
        {
            return Ok(_courseService.GetAllCourses().Select(course => CourseDto.FromModel(course)));
        }
        // Post api/Course
        [HttpPost]
        public ActionResult Create([FromBody] CourseDto value)
        {
            var result = _courseService.CreateCourse(value.ToModel());
            if (result.HasErrors)
                return BadRequest(result.Errors);
            return Accepted();
        }

        // GET api/Course/5
        [HttpGet("{id}")]
        public ActionResult<CourseDto> Get(int id)
        {
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(CourseDto.FromModel(course));
        }
        
        // PUT api/Course/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CourseDto value)
        {
            var updateResult = _courseService.UpdateCourse(value.ToModel());
            if (updateResult.HasErrors)
            {
                return BadRequest(updateResult.Errors);
            }
            return Accepted();
        }

        // DELETE api/Course/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _courseService.DeleteCourse(id);
            return Accepted();
        }
    }
}
