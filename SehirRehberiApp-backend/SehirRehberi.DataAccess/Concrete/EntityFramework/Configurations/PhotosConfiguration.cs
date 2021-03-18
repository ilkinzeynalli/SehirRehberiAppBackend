using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Configurations
{
    public class PhotosConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasKey(h => h.PhotoId);

            builder.Property(p => p.DateAdded).HasColumnType("datetime");
            builder.Property(p => p.DateAdded).HasDefaultValueSql("getDate()");
            builder.Property(p => p.Description).HasMaxLength(255);
            builder.Property(p => p.IsMain).HasDefaultValue(false);
            builder.Property(p => p.PublicId).HasMaxLength(255);


            //Photo one to many relation with City
            builder.HasOne(p => p.City)
                .WithMany(p => p.Photos)
                .HasForeignKey(f => f.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
