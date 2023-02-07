using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    internal class OrderConfiguration : EntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.DateTime).IsConcurrencyToken();

            builder.Property(x => x.Number).HasDefaultValueSql("NEXT VALUE FOR sequences.OrderNumber");

            builder.Property(x => x.Type)/*.HasConversion(x => x.ToString(),
                                                        x => Enum.Parse<OrderType>(x));*/
                                        //.HasConversion<string>();
                                        .HasConversion(new EnumToStringConverter<OrderType>());
        }                       
    }
}
