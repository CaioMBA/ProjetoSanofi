using Domain.Models.GeneralSettings;
using Microsoft.Extensions.Options;
using CrossCutting.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Data.DataBase.SecurityDAO;
using Data.DataBase;
using Domain.Utils;
using Microsoft.Extensions.DependencyInjection;

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
    string? Key = new JwtDAO(new DataBaseDefaultAccess(), new Utils(AppConfigurations)).GetKey() ?? throw new Exception("Secret key not found");

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AppConfigurations.AppName,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
    };

    Key = null;
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
