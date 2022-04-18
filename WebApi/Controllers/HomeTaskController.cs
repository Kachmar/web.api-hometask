using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services;
using WebApi.Dto;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeTask : ControllerBase
    {
        private readonly HomeTaskService _hometaskService;

        public HomeTask(HomeTaskService hometaskService)
        {
            _hometaskService = hometaskService;
        }

        // GET: api/Student
        [HttpGet]
        public ActionResult<IEnumerable<HomeTaskDto>> Get()
        {
            return Ok(_hometaskService.GetAllHomeTasks().Select(task => HomeTaskDto.FromModel(task)));
        }

        // GET api/Student/5
        [HttpGet("{id}")]
        public ActionResult<HomeTaskDto> Get(int id)
        {
            var task = _hometaskService.GetHomeTaskById(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(HomeTaskDto.FromModel(task));
        }
        
        // PUT api/Student/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] HomeTaskDto value)
        {
            var result = _hometaskService.UpdateHomeTask(value.ToModel());
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
            _hometaskService.DeleteHomeTask(id);
            return Accepted();
        }
        [HttpPost]
        public ActionResult Post([FromBody] HomeTaskDto value)
        {
            var insertresult = _hometaskService.CreateHomeTask(value.ToModel());
            if (insertresult.HasErrors)
            {
                return BadRequest(insertresult.Errors);
            }
            return Accepted();
        }
    }
}
