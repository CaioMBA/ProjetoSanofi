using CrossCutting.DependencyInjection;
using Data.DataBase;
using Data.DataBase.SecurityDAO;
using Domain.Models;
using Domain.Models.GeneralSettings;
using Domain.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var AppConfigurations = new AppSettingsModel();
new ConfigureFromConfigurationOptions<AppSettingsModel>(
              builder.Configuration.GetSection("Configurations"))
                  .Configure(AppConfigurations);


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    JwtDAO? jwtDAO = new JwtDAO(new DataBaseDefaultAccess(), new Utils(AppConfigurations));
    string? Key = jwtDAO.GetKey() ?? throw new Exception("Secret key not found");
    string? Issuer = jwtDAO.GetIssuer() ?? throw new Exception("Issuer not found");

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = Issuer,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
    };

    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            ValidatedJwtResponseModel jwtResponseModel = new ValidatedJwtResponseModel()
            {
                Success = false,
                Message = "",
                Token = null,
                Claims = null
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401;
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Append("Token-Expired", "true");
                jwtResponseModel.Message = "Token has expired";
            }
            else if (context.Exception.GetType() == typeof(SecurityTokenInvalidSignatureException))
            {
                jwtResponseModel.Message = "Invalid token signature";
            }
            else if (context.Exception.GetType() == typeof(SecurityTokenInvalidIssuerException))
            {
                jwtResponseModel.Message = "Invalid token issuer";
            }
            else
            {
                jwtResponseModel.Message = "Invalid token";
            }
            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(jwtResponseModel));
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                string result = System.Text.Json.JsonSerializer.Serialize(new ValidatedJwtResponseModel()
                {
                    Success = false,
                    Message = "You are not authorized",
                    Token = null,
                    Claims = null
                });
                return context.Response.WriteAsync(result);
            }
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            string result = System.Text.Json.JsonSerializer.Serialize(new ValidatedJwtResponseModel()
            {
                Success = false,
                Message = "You do not have permission to access this resource",
                Token = null,
                Claims = null
            });
            return context.Response.WriteAsync(result);
        }
    };

    Key = null;
    Issuer = null;
    jwtDAO = null;
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

AllConfigurations.ConfigureDependencies(builder.Services, AppConfigurations);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc($"v{AppConfigurations.AppVersion}", new OpenApiInfo { Title = AppConfigurations.AppName, Version = AppConfigurations.AppVersion.ToString() });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint($"v{AppConfigurations.AppVersion}/swagger.json", AppConfigurations.AppName));
}

app.UseCors(opt => opt.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
