using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface IPhotoService
    {
        Task<IDataResult<List<Photo>>> GetPhotosByCityId(int cityId);
        Task<IDataResult<Photo>> AddPhoto(Photo photo);
        Task<IDataResult<Photo>> GetPhotoById(int id);
    }
}
