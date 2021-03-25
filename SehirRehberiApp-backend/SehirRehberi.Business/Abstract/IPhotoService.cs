using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface IPhotoService
    {
        Task<List<Photo>> GetPhotosByCityId(int cityId);
        Task<Photo> AddPhoto(Photo photo);
        Task<Photo> GetPhotoById(int id);
    }
}
