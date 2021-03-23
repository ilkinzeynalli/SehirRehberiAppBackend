using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Configurations
{
    public class ApplicationUserTokenConfiguration : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
        {
            builder.Property(p => p.ExpireDate).HasColumnType("datetime");
        }
     }
}
