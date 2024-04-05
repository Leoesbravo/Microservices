using LEscogidoShopWebProject.DTO;
using LEscogidoShopWebProject.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LEscogidoShopWebProject.Controllers
{
    public class CuponController : Controller
    {
        private readonly IConfiguration _configuration;

        public CuponController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult GetAll()
        {
            CuponDTO cuponDTO = new CuponDTO();
            cuponDTO.Cupones = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiCupon"]);

                var postTask = client.GetAsync("GetAll");
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var content = result.Content.ReadAsAsync<ResultDTO>();
                    content.Wait();
                    foreach (var  cuponObj in content.Result.Objects)
                    {
                        CuponDTO cupon  = JsonConvert.DeserializeObject<CuponDTO>(cuponObj.ToString());
                        cuponDTO.Cupones.Add(cupon);
                    }
                    return View(cuponDTO);
                }
                else
                {
                    return View();
                }
            }
        }
    }
}
