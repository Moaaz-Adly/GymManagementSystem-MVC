using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configrations
{
    public class GymUserConfigration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(x=>x.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(x=>x.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);



            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidEmailCheck", "Email like '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserValidPhone", "Phone LIKE '01%' AND Phone NOT LIKE '%[^0-9]%' ");
            });

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();

            builder.OwnsOne(x => x.Address, AddressBuilder =>
            {
                AddressBuilder.Property(x=>x.City)
                .HasColumnName("City")
                .HasColumnType("varchar")
                .HasMaxLength(30);
                
                AddressBuilder.Property(x=>x.Street)
                .HasColumnName("Street")
                .HasColumnType("varchar")
                .HasMaxLength(30);

                AddressBuilder.Property(x => x.BuildingNo)
                .HasColumnName("BuildingNo");

            });







        }
    }
}
