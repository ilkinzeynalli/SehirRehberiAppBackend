using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Core.Attributes;
using SehirRehberi.Entities.Concrete;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiRoutePrefix("cities")]
    [AuthorizeRoles(RoleTypes.Admin)]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        //IoC contrainer inversion of control
        //Loosely coupled
        //naming convention
        private readonly ICityService _cityManager;
        private readonly IPhotoService _photoManager;

        public CitiesController(ICityService cityManager,
                                IPhotoService photoManager)
        {
            _cityManager = cityManager;
            _photoManager = photoManager;
        }

        [AllowAnonymous]
        [HttpGet("getAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            var result = await _cityManager.GetAllCitiesWithPhotos();

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("getCityById/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var result = await _cityManager.GetCityById(id);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpGet("getPhotosByCityId/{cityId}")]
        public async Task<IActionResult> GetPhotosByCityId(int cityId)
        {
            var result = await _photoManager.GetPhotosByCityId(cityId);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpPost("addCity")]
        public async Task<IActionResult> AddCity([FromBody] City city)
        {
            var result = await _cityManager.AddCity(city);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
