using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Maps
{
    internal class UserGroupMap : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasKey(x => x.UserGroupId);

            builder.Property(x => x.Code);
            builder.Property(x => x.Description).IsRequired(false);

            builder.ToTable("UserGroup");
        }
    }
}