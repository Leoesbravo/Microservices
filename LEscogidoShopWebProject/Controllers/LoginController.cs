using LEscogidoShopWebProject.DTO;
using LEscogidoShopWebProject.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace LEscogidoShopWebProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ItokenProvider _tokenProvider;

        public LoginController(IConfiguration configuration, ItokenProvider tokenProvider)
        {
            _configuration = configuration;
            _tokenProvider = tokenProvider;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            LoginResponseDTO response = new LoginResponseDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiCupon"]);

                var postTask = client.PostAsJsonAsync<LoginRequestDTO>("Login", model);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var content = result.Content.ReadAsAsync<ResultDTO>();
                    content.Wait();
                    response = JsonConvert.DeserializeObject<LoginResponseDTO>(content.Result.Object.ToString());

                    await SignInUser(response);
                    _tokenProvider.SetToken(response.Token);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("CustomError", "Error al iniciar sesión");
                    return View(response);
                }
            }
        }
        private async Task SignInUser(LoginResponseDTO dto)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(dto.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
