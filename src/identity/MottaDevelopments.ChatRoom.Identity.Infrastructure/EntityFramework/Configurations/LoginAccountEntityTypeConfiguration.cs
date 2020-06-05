using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;

namespace MottaDevelopments.ChatRoom.Identity.Infrastructure.EntityFramework.Configurations
{
    public class LoginAccountEntityTypeConfiguration : IEntityTypeConfiguration<LoginAccount>
    {
        public void Configure(EntityTypeBuilder<LoginAccount> builder)
        {
            builder.Ignore(account => account.DomainEvents);
        }
    }
}