using BackendCrudAngular.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BackendCrudAngular.DTOs;
using BackendCrudAngular.Utilidades;
using BackendCrudAngular.Services.Contrato;
using BackendCrudAngular.Services.Implementacion;
using BackendCrudAngular.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configuración de base de datos
builder.Services.AddDbContext<DbempleadoContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaConexion")));

// inyeccion de dependencias
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

// configuración de automapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile)); // adicionar configuracion de automapper

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin();
        app.AllowAnyHeader();
        app.AllowAnyMethod();
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Peticiones api rest
app.MapGet("/departamento/lista", async (
    IDepartamentoService _departamentoServicio,
    IMapper _mapper
    ) =>
{
    var listaDepartamento = await _departamentoServicio.GetList();
    var listaDepartamentoDTO = _mapper.Map<List<DepartamentoDTO>>(listaDepartamento);

    if (listaDepartamentoDTO.Count > 0)
        return Results.Ok(listaDepartamentoDTO);
    else
        return Results.NotFound();

});

// empleado

//get
app.MapGet("/empleado/lista", async (
    IEmpleadoService _empleadoServicio,
    IMapper _mapper
    ) =>
{
    var listaEmpleado= await _empleadoServicio.GetList();
    var listaEmpleadoDTO = _mapper.Map<List<EmpleadoDTO>>(listaEmpleado);

    if (listaEmpleadoDTO.Count > 0)
        return Results.Ok(listaEmpleadoDTO);
    else
        return Results.NotFound();

});

// post
app.MapPost("/empleado/guardar", async(
    EmpleadoDTO modelo,
    IEmpleadoService _empleadoServicio,
    IMapper _mapper
    ) => 
{
    var _empleado = _mapper.Map<Empleado>(modelo);
    var empleadoCreado = await _empleadoServicio.Add(_empleado);

    if(empleadoCreado.IdEmpleado != 0)
        return Results.Ok(_mapper.Map<EmpleadoDTO>(empleadoCreado));
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);

    });

// put
app.MapPut("/empleado/actualizar/{idEmpleado}", async(
    int idEmpleado,
    EmpleadoDTO modelo,
    IEmpleadoService _empleadoServicio,
    IMapper _mapper
    ) => 
{
    var encontrado = await _empleadoServicio.Get(idEmpleado);
    if( encontrado is null ) return Results.NotFound();

    var _empleado = _mapper.Map<Empleado>(modelo);

    encontrado.NombreCompleto = _empleado.NombreCompleto;
    encontrado.IdDepartamento = _empleado.IdDepartamento;
    encontrado.Sueldo = _empleado.Sueldo;
    encontrado.FechaContrato = _empleado.FechaContrato;

    var respuesta= await _empleadoServicio.Update(encontrado);

    if(respuesta)
        return Results.Ok(_mapper.Map<EmpleadoDTO>(encontrado));
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
});

// delete
app.MapDelete("/empleado/eliminar/{idEmpleado}", async (
    int idEmpleado,    
    IEmpleadoService _empleadoServicio) =>
{
    var encontrado = await _empleadoServicio.Get(idEmpleado);
    if (encontrado is null) return Results.NotFound();

    var respuesta = await _empleadoServicio.Delete(encontrado);

    if (respuesta)
        return Results.Ok();
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
});

#endregion

app.UseCors("NuevaPolitica");

app.Run();

