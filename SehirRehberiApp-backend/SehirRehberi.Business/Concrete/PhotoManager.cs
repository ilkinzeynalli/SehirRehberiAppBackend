using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
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

        public PhotoManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<Photo>> AddPhoto(Photo photo)
        {
            _unitOfWork.Photos.Add(photo);
            await _unitOfWork.Complete();

            return new SuccessDataResult<Photo>(photo, Messages.PhotoAdded);
        }

        public async Task<IDataResult<Photo>> GetPhotoById(int id)
        {
            return new SuccessDataResult<Photo>(await _unitOfWork.Photos.GetEntityById(id));
        }

        public async Task<IDataResult<List<Photo>>> GetPhotosByCityId(int cityId)
        {
            var result = await _unitOfWork.Photos.GetAll(p => p.CityId == cityId).Include(i => i.City).ToListAsync();

            return new SuccessDataResult<List<Photo>>(result);
        }
    }
}
