using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Concrete
{
    public class PhotoManager : IPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PhotoManager> _logger;

        public PhotoManager(IUnitOfWork unitOfWork, ILogger<PhotoManager> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IDataResult<Photo>> AddPhoto(Photo photo)
        {
            try
            {
                _unitOfWork.Photos.Add(photo);
                await _unitOfWork.Complete();

                return new SuccessDataResult<Photo>(photo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }

        }

        public async Task<IDataResult<Photo>> GetPhotoById(int id)
        {
            try
            {
                return new SuccessDataResult<Photo>(await _unitOfWork.Photos.GetEntityById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }

        public async Task<IDataResult<List<Photo>>> GetPhotosByCityId(int cityId)
        {
            try
            {
                var result = await _unitOfWork.Photos.GetAll(p => p.CityId == cityId).Include(i => i.City).ToListAsync();
                return new SuccessDataResult<List<Photo>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }
    }
}
