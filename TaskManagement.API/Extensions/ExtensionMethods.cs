﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskManagement.API.Services;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Models;
using TaskManagement.Repository;
using TaskManagement.Repository.Repositories;
using TaskManagement.Service.Services;
using TaskManagement.Shared.ServiceInterfaces;

namespace TaskManagement.API.Extensions;

public static class ExtensionMethods
{
    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Management API", Version = "v1" });

            // Add JWT Authentication to Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token here: **Bearer {your token}**"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<IBaseRepository<AppTask>, BaseRepository<AppTask>>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IUserContext, UserContextWeb>();
        builder.Services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();
        builder.Services.AddScoped<ITaskCommentService, TaskCommentService>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<ITaskAssignmentRequestRepository, TaskAssignmentRequestRepository>();
        builder.Services.AddScoped<IBaseRepository<BaseNotification>, BaseRepository<BaseNotification>>();
        builder.Services.AddSingleton<TaskGroupTracker>();
    }

    public static void AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<TaskManagementDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        // Configure Identity
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<TaskManagementDBContext>()
            .AddDefaultTokenProviders();

// Configure JWT Authentication
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty);
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Try to get the JWT token from cookies
                        var token = context.Request.Cookies["jwt"];
                        if (!string.IsNullOrEmpty(token)) context.Token = token;
                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void AddCors(this WebApplication app)
    {
        app.UseCors(policy =>
            policy.WithOrigins("http://localhost:5174")
                .WithOrigins("http://localhost:5173")
                .WithOrigins("http://localhost:5175")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    }

    // public static void AddFluentValidation(this WebApplicationBuilder builder)
    // {
    //     builder.Services.AddControllers()
    //         .AddFluentValidation(config =>
    //         {
    //             config.RegisterValidatorsFromAssemblyContaining<CreateTaskDtoValidator>();
    //             config.RegisterValidatorsFromAssemblyContaining<UpdateTaskDtoValidator>();
    //             config.RegisterValidatorsFromAssemblyContaining<UpdateTaskStatusDtoValidator>();
    //         });
    // }
}