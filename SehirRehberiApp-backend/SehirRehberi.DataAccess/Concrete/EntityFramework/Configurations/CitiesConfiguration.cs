using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.DataAccess.Concrete.EntityFramework.Configurations
{
    public class CitiesConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(h => h.CityId);

            builder.Property(p => p.CityName).HasMaxLength(255);
            builder.Property(p => p.CityDescription).HasMaxLength(255);


            //Cities one to many relation with ApplicationUsers
            builder.HasOne(p => p.ApplicationUser)
                .WithMany(p => p.Cities)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //Cities many to one relation with Photos
            builder.HasMany(p => p.Photos)
                .WithOne(p => p.City)
                .HasForeignKey(f => f.CityId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
