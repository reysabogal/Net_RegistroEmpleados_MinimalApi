using BackendCrudAngular.Models;

namespace BackendCrudAngular.Services
{
    public interface IDepartamentoService
    {
        Task<List<Departamento>> GetList();
    }
}
