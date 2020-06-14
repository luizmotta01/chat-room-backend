using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;

namespace MottaDevelopments.ChatRoom.Identity.Infrastructure.EntityFramework.Configurations
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Ignore(account => account.DomainEvents);

        }
    }
}