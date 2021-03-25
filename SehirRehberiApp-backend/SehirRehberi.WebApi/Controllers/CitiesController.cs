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
using SehirRehberi.WebApi.Attributes;
using SehirRehberi.WebApi.Dtos;
using SehirRehberi.WebApi.Models;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiRoutePrefix("cities")]
    [AuthorizeRoles(Roles.User)]
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
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _cityService.GetAllCitiesWithPhotos();

            var result = _mapper.Map<List<CityForListDTO>>(cities);

            return Ok(result);
        }

        [HttpGet]
        [Route("getCityById/{id}")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var city = await _cityService.GetCityById(id);

            var result = _mapper.Map<CityForDetailDTO>(city);

            return Ok(result);
        }

        [HttpGet]
        [Route("getPhotosByCityId/{cityId}")]
        public async Task<IActionResult> GetPhotosByCityId(int cityId)
        {
            var result = await _photoService.GetPhotosByCityId(cityId);

            return Ok(result);
        }

        [HttpPost]
        [Route("addCity")]
        public async Task<IActionResult> AddCity([FromBody] City city)
        {
            var result = await _cityService.AddCity(city);
            return Ok(result);
        }

        
    }
}
