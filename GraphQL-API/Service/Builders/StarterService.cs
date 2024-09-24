using Data.DataBase.Repository;
using Domain.Interfaces;
using System.Reflection;

namespace Service.Builders
{
    public class StarterService : IStarterService
    {
        private DefaultRepository _db;
        private List<TableInformationModel> _DataBaseTables;
        private Assembly _GeneratedAssembly;

        public StarterService(DefaultRepository db)
        {
            _db = db;
            _DataBaseTables = _db.GetAllTableInformations();
            if (_DataBaseTables == null || _DataBaseTables.Count == 0)
            {
                throw new Exception("No tables found in the database");
            }
            _GeneratedAssembly = EntitiesAndTypesGeneratorService.Generate(_DataBaseTables);
        }

        public Assembly GetAssembly()
        {
            return _GeneratedAssembly;
        }

        public List<TableInformationModel> GetTables()
        {
            return _DataBaseTables;
        }
    }
}