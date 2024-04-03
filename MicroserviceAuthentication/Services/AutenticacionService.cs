using MicroserviceAuthentication.DTO;
using MicroserviceAuthentication.Model;
using MicroserviceAuthentication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceAuthentication.Services
{
    public class AutenticacionService : IAutenticacionService //Implementar
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTTokenGenerator _jwtTokenGenerator;
        public AutenticacionService(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IJWTTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            LoginResponseDTO responseDTO = new LoginResponseDTO();
            var usuario = _context.Users.SingleOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
            if (usuario != null)
            {
                //validar password
                bool esValida = await _userManager.CheckPasswordAsync(usuario, model.Password);
                if (esValida)
                {
                    //generar el token
                   var token = _jwtTokenGenerator.GenerateToken(usuario);

                    UsuarioDTO usuarioDTO = new UsuarioDTO();
                    usuarioDTO.UserName = usuario.UserName;
                    usuarioDTO.IdUsuario = usuario.Id;
                    usuarioDTO.Numero = usuario.PhoneNumber;
                    usuarioDTO.Email = usuario.Email;

                    responseDTO.Token = token;
                    responseDTO.UserDto = usuarioDTO;
                }
                else
                {               
                    responseDTO.Token = "";
                    responseDTO.UserDto = null;
                }
            }
            else
            {
                responseDTO.Token = "";
                responseDTO.UserDto = null;
            }
            return responseDTO;
        }
        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = registrationRequestDto.UserName,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                PhoneNumber = registrationRequestDto.Numero
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userResult = _context.Users.First(u => u.UserName == registrationRequestDto.UserName);
                    UsuarioDTO userDto = new UsuarioDTO
                    {
                        UserName = userResult.UserName,
                        Email = userResult.Email,
                        IdUsuario = userResult.Id,
                        Numero = userResult.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.First().Description;
                }
            }
            catch (Exception ex)
            {

            }
            return "Error";
        }
    }
}
