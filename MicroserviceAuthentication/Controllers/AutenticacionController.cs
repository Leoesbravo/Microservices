using MicroserviceAuthentication.DTO;
using MicroserviceAuthentication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AutenticacionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO model)
        {

        }
    }
}
//metodos sincronos y asincronos
//metodos sincronos
//metodos asincronos


