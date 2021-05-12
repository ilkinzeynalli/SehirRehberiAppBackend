using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Core.Attributes;
using SehirRehberi.Core.Utilities.CloudMediaStorages.Cloudinary;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [ApiRoutePrefix("cities/{cityId}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoManager;
       
        public PhotosController(IPhotoService photoManager)
        {
            _photoManager = photoManager;
        }

        [HttpPost("AddPhotoForCity")]
        public async Task<IActionResult> AddPhotoForCity(int cityId, [FromForm] PhotoForCreationDto photoForCreationDTO)
        {
            var result = await _photoManager.AddPhoto(cityId, photoForCreationDTO);

            if (result.Success)
            {
                return CreatedAtRoute("GetPhoto", new { id = result.Data.Id }, result);
            }

            return BadRequest(result);
        }

        [HttpGet("/GetPhoto/{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var result = await _photoManager.GetPhotoById(id);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);

        }
    }
}
