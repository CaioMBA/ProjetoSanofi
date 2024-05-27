using Domain.Models;
using Domain.Models.GeneralSettings;
using Domain.Utils;

namespace Data.DataBase.SqlServerDAO
{
    public class UserDAO
    {
        private DataBaseDefaultAccess _db;
        private DataBaseConnections _con;
        private Utils _utils;

        public UserDAO(DataBaseDefaultAccess db, Utils utils)
        {
            _db = db;
            _utils = utils;
            _con = _utils.GetDataBase("SanofiChallenge");
        }
        public DefaultResponseModel? Create(CreateUserRequestModel request)
        {
            return _db.ExecutaProcFirstOrDefault<DefaultResponseModel>($"spr_api_register_create_user", request, _con).Result;
        }

    }
}
