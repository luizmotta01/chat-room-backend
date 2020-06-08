using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottaDevelopments.ChatRoom.Contacts.Domain.Entities;

namespace MottaDevelopments.ChatRoom.Contacts.Infrastructure.EntityFramework.Configurations
{
    public class UsersEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Ignore(user => user.DomainEvents);
            
            builder.Property(user => user.AccountId).IsRequired();
        }
    }
}