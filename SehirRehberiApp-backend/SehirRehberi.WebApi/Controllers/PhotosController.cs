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
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.WebApi.Attributes;
using SehirRehberi.WebApi.Dtos;
using SehirRehberi.WebApi.Models;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [ApiRoutePrefix("cities/{cityId}/photos")]
    public class PhotosController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private readonly Cloudinary _cloudinary;

        public PhotosController(IOptions<CloudinarySettings> cloudinarySettings, ICityService cityService, IPhotoService photoService, IMapper mapper)
        {
            _photoService = photoService;
            _cityService = cityService;
            _mapper = mapper;

            _cloudinarySettings = cloudinarySettings;

            Account account = new Account(_cloudinarySettings.Value.CloudName, _cloudinarySettings.Value.ApiKey, _cloudinarySettings.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        [HttpPost]
        [Route("AddPhotoForCity")]
        public async Task<IActionResult> AddPhotoForCity(int cityId, [FromForm] PhotoForCreationDTO photoForCreationDTO)
        {
            var city = await _cityService.GetCityWithPhotosById(cityId);

            if (city == null)
                return NotFound("Not founded city");

            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (currentUser != city.UserId)
                return Unauthorized();

            #region Photo upload Cloudinary
            var file = photoForCreationDTO.File;

            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            #endregion

            photoForCreationDTO.Url = uploadResult.Url.ToString();
            photoForCreationDTO.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDTO);

            if (!city.Photos.Any(a => a.IsMain))
            {
                photo.IsMain = true;
            }

            photo.City = city;
            var result = await _photoService.AddPhoto(photo);

            if (result.PhotoId > 0)
            {
                var photoForReturn = _mapper.Map<PhotoForReturnDTO>(photo);
                return CreatedAtRoute("GetPhoto",new { id = photoForReturn.Id },photoForReturn);
            }

            return BadRequest("Coould not add the photo");
        }

        [HttpGet("/GetPhoto/{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromDb = await _photoService.GetPhotoById(id);

            if (photoFromDb == null)
                return NotFound("Photo not found");

            var photo = _mapper.Map<PhotoForReturnDTO>(photoFromDb);

            return Ok(photo);
        }
    }
}
