using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SehirRehberi.Business.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;

namespace SehirRehberi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValueService _valueService;
        public ValuesController(IValueService valueService)
        {
            _valueService = valueService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _valueService.GetAllValues());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _valueService.GetValueById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Value model)
        {

            return Ok(await _valueService.AddValue(model));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Value model)
        {
            return Ok(await _valueService.UpdateValue(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _valueService.GetValueById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(await _valueService.DeleteValue(result));
        }

        [HttpGet("arrays")]
        public async Task<IActionResult> GetArrays()
        {
            return Ok("deneme");
        }
    }
}
