using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Maps
{
    internal class UserStateMap : IEntityTypeConfiguration<UserState>
    {
        public void Configure(EntityTypeBuilder<UserState> builder)
        {
            builder.HasKey(x => x.UserStateId);

            builder.Property(x => x.Code);
            builder.Property(x => x.Description).IsRequired(false);

            builder.ToTable("UserState");
        }
    }
}