using Microsoft.EntityFrameworkCore;
using System;
using TaskManagement.Domain.Models;
using TaskManagement.Repository;
using TaskManagement.Service.Interfaces;
using TaskManagement.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Register DbContext with dependency injection
builder.Services.AddDbContext<TaskManagementDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IBaseRepository<AppTask>, BaseRepository<AppTask>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
