﻿namespace MicroserviceAuthentication.DTO
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Numero { get; set; }
        public string Password { get; set; }
        public string? RoleName { get; set; }
    }
}
