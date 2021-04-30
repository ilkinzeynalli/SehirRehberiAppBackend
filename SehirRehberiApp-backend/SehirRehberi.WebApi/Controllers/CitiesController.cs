using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SehirRehberi.Business.Abstract;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using SehirRehberi.WebApi.Attributes;
using SehirRehberi.WebApi.Models;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiRoutePrefix("cities")]
    [AuthorizeRoles(RoleTypes.Admin)]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IPhotoService _photoService;

        private readonly IMapper _mapper;
        public CitiesController(IMapper mapper,
                                ICityService cityService,
                                IPhotoService photoService)
        {
            _cityService = cityService;
            _photoService = photoService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getAllCities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCities()
        {

            var result = await _cityService.GetAllCitiesWithPhotos();

            if (result.Success)
            {
                return Ok(_mapper.Map<List<CityForListDto>>(result.Data));
            }

            return BadRequest(result.Message);
        }

        [HttpGet]
        [Route("getCityById/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var result = await _cityService.GetCityById(id);

            if (result.Success)
            {
                return Ok(_mapper.Map<CityForDetailDto>(result.Data));
            }

            return BadRequest(result.Message);
        }

        [HttpGet]
        [Route("getPhotosByCityId/{cityId}")]
        public async Task<IActionResult> GetPhotosByCityId(int cityId)
        {
            var result = await _photoService.GetPhotosByCityId(cityId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("addCity")]
        public async Task<IActionResult> AddCity([FromBody] City city)
        {
            var result = await _cityService.AddCity(city);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);

        }


    }
}
