using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface IPhotoService
    {
        Task<IDataResult<List<Photo>>> GetPhotosByCityId(int cityId);
        Task<IDataResult<PhotoForReturnDto>> AddPhoto(int cityId,PhotoForCreationDto photoForCreationDTO);
        Task<IDataResult<PhotoForReturnDto>> GetPhotoById(int id);
    }
}
