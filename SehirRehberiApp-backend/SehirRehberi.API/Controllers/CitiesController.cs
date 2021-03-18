using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.WebApi.Attributes;
using SehirRehberi.WebApi.Dtos;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiRoutePrefix("cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CitiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("getAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            var result = await _unitOfWork.Cities.GetAll().Select(s => new CityForListDto {
                                                                            Id = s.CityId,
                                                                            Name = s.CityName,
                                                                            Description = s.CityDescription,
                                                                            PhotoUrl = s.Photos.FirstOrDefault(w=>w.IsMain).PhotoUrl
                                                                    })
                                                          .ToListAsync();

            return Ok(result);
        }
    }
}
