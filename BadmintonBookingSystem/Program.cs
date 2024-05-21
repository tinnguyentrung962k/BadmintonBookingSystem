using BadmintonBookingSystem.DataAccessLayer.Context;
using BadmintonBookingSystem.Repository.Interface;
using BadmintonBookingSystem.Repository.Repository;
using BadmintonBookingSystem.Service.Interface;
using BadmintonBookingSystem.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IBadmintonCenterRepository, BadmintonCenterRepository>();
builder.Services.AddScoped<IBadmintonCenterService, BadmintonCenterService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
