using MicroserviceAuthentication.DTO;
using MicroserviceAuthentication.Model;
using MicroserviceAuthentication.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionService _authService;
        public AutenticacionController(IAutenticacionService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO model)
        {
            ResultDTO resultDTO = new ResultDTO();
            var result = await _authService.Login(model);
            if (result.UserDto == null)
            {
                resultDTO.Correct = false;
                resultDTO.ErrorMessage = "nombre o contraseña incorrecto";
                return BadRequest(resultDTO);
            }
            resultDTO.Object = result;
            return Ok(resultDTO);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            ResultDTO resultDTO = new ResultDTO();
            var errorMessage = await _authService.Register(registrationRequestDto);
            if (errorMessage == null)
            {
                resultDTO.Correct = false;
                resultDTO.ErrorMessage = errorMessage;
                return BadRequest(resultDTO);
            }
            else
            {
                return Ok(resultDTO);
            }

        }
    }
}
//metodos sincronos y asincronos
//metodos sincronos
//metodos asincronos


