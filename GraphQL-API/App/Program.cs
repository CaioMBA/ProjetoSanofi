using Domain.Models.GeneralSettings;
using Microsoft.Extensions.Options;
using CrossCutting.DependencyInjection;
using Microsoft.OpenApi.Models;
using Domain.Interfaces;
using System.Reflection;
using GraphQL;
using Service.GraphServices;
using GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var AppConfigurations = new AppSettingsModel();
new ConfigureFromConfigurationOptions<AppSettingsModel>(
              builder.Configuration.GetSection("Configurations"))
                  .Configure(AppConfigurations);

AllConfigurations.ConfigureDependencies(builder.Services, AppConfigurations);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc($"v{AppConfigurations.AppVersion}", new OpenApiInfo { Title = AppConfigurations.AppName, Version = AppConfigurations.AppVersion.ToString() });
});

ServiceProvider? serviceProvider = builder.Services.BuildServiceProvider();
IStarterService? starterService = serviceProvider.GetService<IStarterService>();
Assembly? generatedAssembly = starterService.GetAssembly();
List<TableInformationModel>? TableInformation = starterService.GetTables();

builder.Services.AddSingleton(generatedAssembly);
builder.Services.AddSingleton<List<TableInformationModel>>(TableInformation);

builder.Services.AddGraphQL(builder =>
{
    builder
        .AddSystemTextJson()
        .AddSchema<SchemasService>()
        .AddGraphTypes(typeof(SchemasService).Assembly)
        .AddGraphTypes(generatedAssembly);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
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

app.UseAuthorization();

app.MapControllers();

app.UseGraphQL<ISchema>("/graphql");
app.UseGraphQLPlayground("/playground");

app.Run();
