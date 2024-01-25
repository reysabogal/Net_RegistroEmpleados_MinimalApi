using AutoMapper;
using BackendCrudAngular.DTOs;
using BackendCrudAngular.Models;
using System.Globalization;

namespace BackendCrudAngular.Utilidades
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() 
        {
            #region Departamento
            CreateMap<Departamento,DepartamentoDTO>().ReverseMap(); // se configura para que el modelo departamento se tome para dto
            #endregion

            #region Empleado
            CreateMap<Empleado, EmpleadoDTO>()
                .ForMember(destino =>
                destino.NombreDepartamento,
                opt => opt.MapFrom(origen => origen.IdDepartamentoNavigation.Nombre)) // se utiliza en la relación para mostrar el nombre en el Dto del empleado
                .ForMember(destino =>
                destino.FechaContrato,
                opt => opt.MapFrom(origen => origen.FechaContrato.Value.ToString("dd/MM/yyyy")) // se utiliza para convertir a String el formato de fecha del modelo al dto.
                );

            CreateMap<EmpleadoDTO, Empleado>()
                .ForMember(destino => 
                destino.IdDepartamentoNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.FechaContrato,
                opt => opt.MapFrom(origen => DateTime.ParseExact(origen.FechaContrato,"dd/MM/yyyy",CultureInfo.InvariantCulture)) // conversion de formato de fecha
                );

            #endregion
        }
    }
}
