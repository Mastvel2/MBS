using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using MBS.Application.Providers;
using MBS.Application.Services;
using MBS.Domain.Repositories;
using MBS.Domain.Services;
using MBS.Host.Hubs;
using MBS.Host.Middlewares;
using MBS.Host.Services;
using MBS.Host.Settings;
using MBS.Infrastructure;
using MBS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

const string corsPolicy = "appPolicy";
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, policyBuilder =>
        policyBuilder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Headers.Authorization.SingleOrDefault();
                var path = context.HttpContext.Request.Path.Value;
                if (!string.IsNullOrEmpty(accessToken) && path?.StartsWith("/ws") == true)
                {
                    context.Token = accessToken.Replace("Bearer ", string.Empty);
                }

                return Task.CompletedTask;
            }
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
builder.Services.AddSingleton<IMessageNotificationService, MessageNotificationService>();
builder.Services.AddSingleton<ITokenFactory, JwtTokenFactory>();
builder.Services.AddSingleton<IUserActivityProvider, UserActivityProvider>();
builder.Services.AddHostedService<UserActivityBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicy)
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<UserActivityMiddleware>()
    .UseEndpoints(endpoints => endpoints.MapControllers())
    .UseHttpsRedirection();
app.MapHub<MessageHub>("/ws/messages");
app.MapControllers();
await app.RunAsync();