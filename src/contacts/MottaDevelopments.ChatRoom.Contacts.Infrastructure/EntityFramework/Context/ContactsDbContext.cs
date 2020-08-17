using MediatR;
using Microsoft.EntityFrameworkCore;
using MottaDevelopments.ChatRoom.Contacts.Domain.Entities;
using MottaDevelopments.ChatRoom.Contacts.Infrastructure.EntityFramework.Configurations;
using MottaDevelopments.MicroServices.Infrastructure.EfCore.Context;


namespace MottaDevelopments.ChatRoom.Contacts.Infrastructure.EntityFramework.Context
{
    public class ContactsDbContext : DbContextBase
    {
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options, IMediator mediator) : base(options, mediator)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersEntityTypeConfiguration());
        }
    }
}