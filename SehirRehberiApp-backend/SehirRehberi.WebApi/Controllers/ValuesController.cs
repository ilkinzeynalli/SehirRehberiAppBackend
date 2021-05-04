using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SehirRehberi.Business.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.WebApi.Attributes;

namespace SehirRehberi.API.Controllers
{
    [ApiRoutePrefix("values")]
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
            var result = await _valueService.GetAllValues();

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _valueService.GetValueById(id);

            if (result.Success)
                return Ok(result);

            return NotFound(result);

        }
       
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Value model)
        {
            var result = await _valueService.AddValue(model);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Value model)
        {
            var result = await _valueService.UpdateValue(model);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _valueService.GetValueById(id);

            if (result.Success)
            {
                var dataForDelete = await _valueService.DeleteValue(result.Data);

                if (dataForDelete.Success)
                    return Ok(dataForDelete);

                return BadRequest(dataForDelete);
            }

            return NotFound(result);
        }
    }
}
