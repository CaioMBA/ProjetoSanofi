using Domain.Models;
using Domain.Models.GeneralSettings;
using Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataBase.SecurityDAO
{
    public class JwtDAO
    {
        private DataBaseDefaultAccess _db;
        private DataBaseConnections _con;
        private Utils _utils;
        public JwtDAO(DataBaseDefaultAccess db, Utils utils)
        {
            _db = db;
            _utils = utils;
            _con = _utils.GetDataBase("SanofiChallenge");
        }

        public string? GetKey()
        {
            return _db.ExecutaQueryFirstOrDefault<string>($"SELECT TOP 1 [KEY] FROM SecurityKeys WHERE [Type] = 'JWT' and Active = 1;", null, _con).Result;
        }

        public UserInfoResponseModel? GetInfo(string Login, string Password)
        {
            return _db.ExecutaQueryFirstOrDefault<UserInfoResponseModel>($"SELECT TOP 1 UserID, Name, Login, DateCreation, Active FROM Users WHERE Login = '{Login}' AND Password = '{Password}'", null, _con).Result;
        }

        public List<UserPermissionResponseModel>? GetPermisions(long UserId)
        {
            return _db.ExecutaProc<UserPermissionResponseModel>("spr_api_auth_user_permissions", new { UserID = UserId }, _con).Result;
        }
    }
}
