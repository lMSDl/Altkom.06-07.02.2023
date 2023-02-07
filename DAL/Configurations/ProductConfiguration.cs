using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    internal class ProductConfiguration : EntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Order).WithMany(x => x.Products).IsRequired();

            builder.Property(x => x.Timestamp).IsRowVersion();

            builder.Property(x => x.Price).HasDefaultValue(99.9);

            builder.Property(x => x.Description).HasComputedColumnSql("[s_Name] + ' ' + STR([Price]) + 'zł'", stored: true);

            //wskazanie nietypowej nazwy backfielda dla property
            //typowe nazwy: <nazwa>, _<nazwa>, m_<nazwa>
            builder.Property(x => x.Name).HasField("n_name");
        }
    }
}
