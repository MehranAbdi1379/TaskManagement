using TaskManagement.API.Extensions;
using TaskManagement.API.Hubs;
using TaskManagement.API.Middlewares;
using TaskManagement.Shared.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.AddDbContext();

builder.Services.AddSignalR();

builder.AddJwtAuthentication();

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(TaskProfile).Assembly);

builder.AddDependencies();

builder.Services.AddEndpointsApiExplorer();

builder.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.AddCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<NotificationHub>("/hub");

app.MapControllers();

app.Run();