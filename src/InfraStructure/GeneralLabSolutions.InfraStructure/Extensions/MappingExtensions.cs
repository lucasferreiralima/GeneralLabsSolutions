using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLabSolutions.Domain.Enums
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public static class MappingExtensions
    {
        public static PropertyBuilder<TEnum> HasEnumConversion<TEnum>(this PropertyBuilder<TEnum> propertyBuilder)
            where TEnum : struct, Enum
        {
            return propertyBuilder
                .HasConversion(
                    v => v.ToString(),
                    v => (TEnum)Enum.Parse(typeof(TEnum), v))
                .HasColumnType("varchar(20)")
                .IsRequired();
        }
    }
}
