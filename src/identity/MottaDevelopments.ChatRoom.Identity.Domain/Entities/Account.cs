using System;
using System.Collections.Generic;
using MottaDevelopments.MicroServices.Domain.Entities;

namespace MottaDevelopments.ChatRoom.Identity.Domain.Entities
{
    public class Account : Entity
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public string Email { get; private set; }

        public ICollection<RefreshToken> RefreshTokens { get; private set; }  = new List<RefreshToken>();

        public void ChangePassword(string newPassword) => Password = newPassword;

    }
}