using MicroserviceAuthentication.DTO;

namespace MicroserviceAuthentication.Services.Interfaces
{
    public interface IAutenticacionService
    {
        //interfaces
        //contrato, estable de estructuras de clases
        //contiene metodos abstractos(sin implementacion, si ejecucion)
        public  Task<LoginResponseDTO> Login(LoginRequestDTO model);
    }
}
