using Data.DataBase.Repository;
using Domain.DTOs;
using GraphQL;
using GraphQL.Types;
using Service.Extensions;
using System.Reflection;

namespace Service.GraphServices
{
    public class QuerysService : ObjectGraphType
    {
        private DefaultRepository _db;
        private List<TableInformationModel> _DataBaseTables;
        private Assembly _generatedEntitiesAndTypes;

        public QuerysService(DefaultRepository db,
                             List<TableInformationModel> dataBaseTables,
                             Assembly generatedEntitiesAndTypes)
        {
            _db = db;
            _DataBaseTables = dataBaseTables;
            _generatedEntitiesAndTypes = generatedEntitiesAndTypes;

            foreach (TableInformationModel table in _DataBaseTables)
            {
                if (table == null || table.COLUMNS == null || table.COLUMNS.Count == 0)
                {
                    continue;
                }

                Type? dynamicEntity = _generatedEntitiesAndTypes.GetType($"GeneratedEntitiesAndTypes.{table.TABLE_NAME}Entity");
                Type? graphQLType = _generatedEntitiesAndTypes.GetType($"GeneratedEntitiesAndTypes.{table.TABLE_NAME}Type");

                if (dynamicEntity == null || graphQLType == null)
                {
                    continue;
                }

                Type graphQLListType = typeof(ListGraphType<>).MakeGenericType(graphQLType);

                QueryArguments? queryArguments = new QueryArguments(
                    table.COLUMNS
                    .Where(column => (bool)column.IsFilterable!)
                    .Select(column =>
                        new QueryArgument<StringGraphType>
                        {
                            Name = column.Name,
                            Description = $"Argument for {column.Name}"
                        }).ToArray());

                Field(graphQLListType,
                    table.TABLE_NAME,
                    arguments: queryArguments,
                    resolve: context =>
                    {
                        try
                        {
                            // Create parameters dynamically based on the context and dynamic entity
                            var parameters = CreateParameters(dynamicEntity, context);

                            // Invoke the GetDirectEntity method dynamically
                            return GetDirectEntityDynamically(dynamicEntity, parameters);
                        }
                        catch (Exception ex)
                        {
                            // Log the error
                            Console.Error.WriteLine($"Error resolving {table.TABLE_NAME}: {ex.Message}");
                            throw new ExecutionError($"Error resolving {table.TABLE_NAME}", ex);
                        }
                    }
                    );
            }
        }

        public dynamic? GetDirectEntityDynamically(Type entityType, DataToRepositoryDTO parameters)
        {
            var method = typeof(DefaultRepository).GetMethod("GetDirectEntity");
            var genericMethod = method.MakeGenericMethod(entityType);

            return genericMethod.Invoke(_db, new object[] { parameters });
        }

        public DataToRepositoryDTO CreateParameters(Type entityType, IResolveFieldContext context)
        {
            // cria um dic com todos os parametros passados na query, este sera usado como base para produzir o Where SQL
            var objFilters = new Dictionary<string, object>();
            var properties = entityType.GetProperties();
            foreach (var property in properties)
            {
                var argumentName = property.Name;// Assume que os nomes das propriedades são iguais aos argumentos
                var argumentValue = context.GetArgument<string>(argumentName);
                if (argumentValue != null)
                    objFilters[property.Name] = argumentValue;
            }

            return new DataToRepositoryDTO()
            {
                Parameters = objFilters.ToDynamicParameters(), // "Parameters"para o dapper dapper
                Where = objFilters.ToSQLWhere(), // cria o where direto ou com as variaveis para ser ultilizado pelos "Parameters" criado acima no dapper
                TableName = context.FieldAst.Name.StringValue,
                Columns = String.Join(", ", context.SubFields.Keys.Where(x => properties.Any(y => y.Name.ToLower().Trim() == x.ToLower().Trim())))
            };
        }
    }
}
