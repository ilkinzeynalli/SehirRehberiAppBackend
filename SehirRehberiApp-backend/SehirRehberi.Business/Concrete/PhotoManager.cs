using Microsoft.EntityFrameworkCore;
using SehirRehberi.Business.Abstract;
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

        public async Task<Photo> AddPhoto(Photo photo)
        {
            _unitOfWork.Photos.Add(photo);
            await _unitOfWork.Complete();

            return photo;
        }

        public async Task<Photo> GetPhotoById(int id)
        {
           return await _unitOfWork.Photos.GetEntityById(id);
        }

        public async Task<List<Photo>> GetPhotosByCityId(int cityId)
        {
            return await _unitOfWork.Photos.GetAll(p => p.CityId == cityId).Include(i => i.City).ToListAsync();
        }
    }
}
