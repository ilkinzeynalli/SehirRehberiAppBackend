using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Repositories
{
    public class EfPhotoRepository : EfGenericRepositoryBase<Photo>, IPhotoRepository
    {
        public EfPhotoRepository(ApplicationIdentityDbContext context)
            :base(context)
        {

        }

        public ApplicationIdentityDbContext Context
        {
            get
            {
                return _context as ApplicationIdentityDbContext;
            }
        }
    }
}
