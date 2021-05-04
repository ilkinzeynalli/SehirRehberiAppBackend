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
using SehirRehberi.Core.Utilities.Results;
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
        //IoC contrainer inversion of control
        //Loosely coupled
        //naming convention
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

        [AllowAnonymous]
        [HttpGet("getAllCities")]
        public async Task<IActionResult> GetAllCities()
        {

            var result = await _cityService.GetAllCitiesWithPhotos();

            if (result.Success)
            {
                var mappedData = _mapper.Map<List<CityForListDto>>(result.Data);

                return Ok(new SuccessDataResult<List<CityForListDto>>(mappedData,result.Message));
            }

            return BadRequest(result);
        }

        [HttpGet("getCityById/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var result = await _cityService.GetCityById(id);

            if (result.Success)
            {
                var mappedData = _mapper.Map<CityForDetailDto>(result.Data);

                return Ok(new SuccessDataResult<CityForDetailDto>(mappedData, result.Message));
            }

            return NotFound(result);
        }

        [HttpGet("getPhotosByCityId/{cityId}")]
        public async Task<IActionResult> GetPhotosByCityId(int cityId)
        {
            var result = await _photoService.GetPhotosByCityId(cityId);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpPost("addCity")]
        public async Task<IActionResult> AddCity([FromBody] City city)
        {
            var result = await _cityService.AddCity(city);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


    }
}
