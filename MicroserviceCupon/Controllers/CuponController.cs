using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroserviceCupon.Model;
using MicroserviceCupon.DTO;

namespace MicroserviceCupon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CuponController(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            ResultDTO result = new ResultDTO();
            try
            {
                var listCupones = _context.Cupones.ToList();
                if (listCupones != null && listCupones.Count > 0)
                {

                    result.Objects = new List<object>();
                    foreach (var cupon in listCupones)
                    {
                        CuponDTO dtoCupon = new CuponDTO();
                        dtoCupon.IdCupon = cupon.IdCupon;
                        dtoCupon.CantidadMinima = cupon.CantidadMinima;
                        dtoCupon.Descuento = cupon.Descuento;
                        dtoCupon.Codigo = cupon.Codigo;

                        result.Objects.Add(dtoCupon);
                    }

                    return Ok(result);
                }
                else
                {
                    return BadRequest("Ocurrio un error");
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Exception = ex;
                result.ErrorMessage = ex.Message;
                return BadRequest(result);
            }
        }
        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] CuponDTO cuponDTO)
        {
            ResultDTO result = new ResultDTO();
            try
            {
                Cupon cupon = new Cupon
                {
                    CantidadMinima = cuponDTO.CantidadMinima,
                    Descuento = cuponDTO.Descuento,
                    Codigo = cuponDTO.Codigo
                };

                _context.Cupones.Add(cupon);
                int rowsAfectted = _context.SaveChanges();
                if (rowsAfectted > 0)
                {
                    return Ok(result);
                }
                else
                {
                    result.Correct = false;
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Exception = ex;
                result.ErrorMessage = ex.Message;
                return BadRequest(result);
            }
        }
        [HttpGet]
        [Route("GetById{idCupon}")]
        public IActionResult GetById(int idCupon)
        {
            ResultDTO result = new ResultDTO();
            try 
            {
                //expresion lambda
                //
                var cupon = _context.Cupones.FirstOrDefault(u => u.IdCupon == idCupon);

                CuponDTO cuponDTO = new CuponDTO();
                cuponDTO.IdCupon = cupon.IdCupon;
                cuponDTO.CantidadMinima = cupon.CantidadMinima;
                cuponDTO.Descuento = cupon.Descuento;

                result.Object = cuponDTO;

                return Ok(result);

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Exception = ex;
                result.ErrorMessage = ex.Message;
                return BadRequest(result);
            }
        }
    }
}
