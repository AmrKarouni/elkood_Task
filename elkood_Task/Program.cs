using ElKood.Application.Configuration;
using ElKood.Application.Interfaces;
using ElKood.Application.Services;
using ElKood.Application.Shared.Models.Inputs;
using ElKood.Application.Shared.Models.Inputs.Identity;
using ElKood.Core.Abstractions;
using ElKood.Core.Entities.Identity;
using ElKood.Infrastructure;
using ElKood.Infrastructure.UnitOfWork;
using elkood_Task.MappingProfiles;
using elkood_Task.Middlewares;
using elkood_Task.Repository;
using elkood_Task.SeedExtension;
using elkood_Task.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("LocalConnection");

builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JWT"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

////
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAppLogService, AppLogService>();
builder.Services.AddScoped<ITaskCategoriesService, TaskCategoriesService>();
builder.Services.AddScoped<IToDoTasksService, ToDoTasksService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(connectionString
        , x => x.MigrationsAssembly("ElKood.Infrastructure"));
});

builder.Services.AddAutoMapper(typeof(ApplicationUserToUserDtoProfile));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                    };
                });

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials();
}));

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });

builder.Services.AddScoped<IValidator<LoginInput>, LoginInputValidator>();
builder.Services.AddScoped<IValidator<RegistrationInput>, RegistrationInputValidator>();
builder.Services.AddScoped<IValidator<TaskCategoryInput>, TaskCategoryInputValidator>();
builder.Services.AddScoped<IValidator<ToDoTaskInput>, ToDoTaskInputValidator>();

builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await services.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger UI URL");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
