using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using MBS.Domain.Entities;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using MBS.Host.ApplicationServices;
using MBS.Host.InfrastructureServices;
using MBS.Host.Middlewares;
using MBS.Host.Providers;
using MBS.Host.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builderP =>
    {
        builderP.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("create_user", configure => configure.RequireRole("admin"));
});
builder.Services.AddControllers().AddJsonOptions(configure =>
{
    configure.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserIdentityRepository, UserIdentityRepository>();
builder.Services.AddScoped<IUserIdentityService, UserIdentityService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddSingleton<ITokenFactory, JwtTokenFactory>();
builder.Services.AddSingleton<IUserStatusProvider, UserStatusProvider>();
builder.Services.AddHostedService<UserActivityBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader())
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<UserStatusMiddleware>()
    .UseEndpoints(endpoints => endpoints.MapControllers())
    .UseHttpsRedirection();

app.MapControllers();
await app.RunAsync();