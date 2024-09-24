using Dapper;
using Domain.DTOs;
using Domain.Models.GeneralSettings;
using Domain.Utils;

namespace Data.DataBase.Repository
{
    public class DefaultRepository
    {
        private DataBaseDefaultAccess _db;
        private DataBaseConnections _con;
        private Utils _utils;
        public DefaultRepository(DataBaseDefaultAccess db, Utils utils)
        {
            _db = db;
            _utils = utils;
            _con = _utils.GetDataBase("DEFAULT");
        }

        public List<TableInformationModel>? GetAllTableInformations()
        {
            SqlMapper.AddTypeHandler(new JsonTypeHandler<Table_Column>());

            return _db.ExecutaProc<TableInformationModel>(
                sQuery: $"spr_return_all_tables",
                parametro: null,
                conexao: _con
                ).Result;
        }

        public List<T>? GetDirectEntity<T>(DataToRepositoryDTO parameters)
        {
            return _db.ExecutaQuery<T>(
                sQuery: $"select {parameters.Columns} from {parameters.TableName} {parameters.Where}",
                parametro: parameters.Parameters,
                conexao: _con
                ).Result;
        }
    }
}
