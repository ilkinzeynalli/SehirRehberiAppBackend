using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Business.ValidationRules.FluentValidation;
using SehirRehberi.Core.Aspects.Autofac.Validation;
using SehirRehberi.Core.Utilities.Business;
using SehirRehberi.Core.Utilities.CloudMediaStorages.Cloudinary;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Concrete
{
    public class PhotoManager : IPhotoService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICityService _cityManager;

        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private readonly Cloudinary _cloudinary;

        public PhotoManager(IMapper mapper,IUnitOfWork unitOfWork, ICityService cityManager, IOptions<CloudinarySettings> cloudinarySettings)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cityManager = cityManager;

            _cloudinarySettings = cloudinarySettings;

            Account account = new Account(_cloudinarySettings.Value.CloudName, _cloudinarySettings.Value.ApiKey, _cloudinarySettings.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        [ValidationAspect(typeof(PhotoForCreationDtoValidator))]
        public async Task<IDataResult<PhotoForReturnDto>> AddPhoto(int cityId,PhotoForCreationDto photoForCreationDTO)
        {
            var city = await ExistCity(cityId);

            if (!city.Success)
            {
                return new ErrorDataResult<PhotoForReturnDto>(city.Message);
            }

            var uploadResult = PhotoUploadCloudinary(photoForCreationDTO.File);
            if (!uploadResult.Success)
            {
                new ErrorDataResult<PhotoForReturnDto>(Messages.CloudeCanNotUpload);
            }

            photoForCreationDTO.Url = uploadResult.Data.Url.ToString();
            photoForCreationDTO.PublicId = uploadResult.Data.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDTO);
            photo.City = city.Data;

            AssingAsMainPhoto(city.Data, photo);

            _unitOfWork.Photos.Add(photo);
            await _unitOfWork.Complete();

            var photoForReturn = _mapper.Map<PhotoForReturnDto>(photo);

            return new SuccessDataResult<PhotoForReturnDto>(photoForReturn, Messages.PhotoAdded);
        }

        public async Task<IDataResult<PhotoForReturnDto>> GetPhotoById(int id)
        {
            var photo = await _unitOfWork.Photos.GetEntityById(id);

            if (photo != null)
            {
                var mappedPhoto = _mapper.Map<PhotoForReturnDto>(photo);
                return new SuccessDataResult<PhotoForReturnDto>(mappedPhoto);
            }

            return new ErrorDataResult<PhotoForReturnDto>(Messages.PhotoNotFound);
        }

        public async Task<IDataResult<List<Photo>>> GetPhotosByCityId(int cityId)
        {
            var result = await _unitOfWork.Photos.GetAll(p => p.CityId == cityId).Include(i => i.City).ToListAsync();

            return new SuccessDataResult<List<Photo>>(result);
        }

        private IDataResult<ImageUploadResult> PhotoUploadCloudinary(IFormFile file)
        {
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

                    return new SuccessDataResult<ImageUploadResult>(uploadResult);
                }
            }

            return new ErrorDataResult<ImageUploadResult>();

        }
        private async Task<IDataResult<City>> ExistCity(int cityId)
        {
            var city = await _cityManager.GetCityWithPhotosById(cityId);

            if (city == null)
                return new ErrorDataResult<City>(Messages.CityNotFound);

            return city;
        }

        private IResult AssingAsMainPhoto(City city,Photo photo)
        {
            if (!city.Photos.Any(a => a.IsMain))
            {
                photo.IsMain = true;
            }

            return new SuccessResult();
        }
    }
}
