using MicroserviceAuthentication.DTO;
using MicroserviceAuthentication.Model;
using MicroserviceAuthentication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MicroserviceAuthentication.Services
{
    public class AutenticacionService : IAutenticacionService //Implementar
    {
        private readonly AppDbContext _context;
        public AutenticacionService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            _context.Users.SingleOrDefault(u => u.UserName == model.UserName);
        }
    }
}
