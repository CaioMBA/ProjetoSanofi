using AutoMapper;
using Data.API;
using Data.DataBase;
using Data.DataBase.Repository;
using Domain.Interfaces;
using Domain.Mapping;
using Domain.Models.GeneralSettings;
using Domain.Utils;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Service.Builders;
using Service.GraphServices;

namespace CrossCutting.DependencyInjection
{
    public class AllConfigurations
    {
        public static void ConfigureDependencies(IServiceCollection serviceCollection, AppSettingsModel appConfig)
        {
            serviceCollection.AddSingleton(appConfig);
            ConfigureAutoMapper(serviceCollection);
            ConfigureDependenciesService(serviceCollection);
            ConfigureDependenciesRepository(serviceCollection);
            ConfigureDependenciesExtras(serviceCollection);
        }

        public static void ConfigureAutoMapper(IServiceCollection serviceCollection)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DtoToModel());
            });
            IMapper mapper = config.CreateMapper();
            serviceCollection.AddSingleton(mapper);
        }

        public static void ConfigureDependenciesService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IStarterService, StarterService>();
            //GraphQL dependencies
            serviceCollection.AddScoped<QuerysService>();
            serviceCollection.AddScoped<ISchema, SchemasService>();
        }

        public static void ConfigureDependenciesRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ApiDefaultAccess>();
            serviceCollection.AddTransient<DataBaseDefaultAccess>();
            serviceCollection.AddTransient<DefaultRepository>();
        }

        public static void ConfigureDependenciesExtras(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<Utils>();
        }
    }
}
