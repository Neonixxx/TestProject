using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Maps
{
    internal class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.Login).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.CreatedDate);

            builder.HasOne(x => x.UserGroup)
                .WithMany(x => x.Users)
                .HasForeignKey(t => t.UserGroupId)
                .IsRequired();

            builder.HasOne(x => x.UserState)
                .WithMany(x => x.Users)
                .HasForeignKey(t => t.UserStateId)
                .IsRequired();

            builder.HasIndex(x => x.Login);

            builder.ToTable("User");
        }
    }
}