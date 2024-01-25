using Microsoft.EntityFrameworkCore;
using BackendCrudAngular.Services.Contrato;
using BackendCrudAngular.Models;

namespace BackendCrudAngular.Services.Implementacion
{
    public class DepartamentoService : IDepartamentoService
    {
        private DbempleadoContext _dbContext;

        public DepartamentoService(DbempleadoContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Departamento>> GetList()
        {
            try
            {
                List<Departamento> lista = new List<Departamento>();
                lista = await _dbContext.Departamentos.ToListAsync();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
