using System;
using MottaDevelopments.ChatRoom.Identity.Domain.Entities;
using Newtonsoft.Json;

namespace MottaDevelopments.ChatRoom.Identity.Application.Models
{
    public class AuthenticationResponse
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string JwtToken { get; set; }

        [JsonIgnore] 
        public string RefreshToken { get; set; }

        public AuthenticationResponse(Account username, string jwtToken, string refreshToken)
        {
            Id = username.Id;
            Username = username.Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}