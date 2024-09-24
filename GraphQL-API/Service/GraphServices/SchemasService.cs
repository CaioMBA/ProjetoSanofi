using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Service.GraphServices
{
    public class SchemasService : Schema
    {
        public SchemasService(IServiceProvider resolver) : base(resolver)
        {
            Query = resolver.GetService<QuerysService>();
        }
    }
}
