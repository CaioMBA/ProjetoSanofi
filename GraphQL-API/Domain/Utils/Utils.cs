using Domain.Models.GeneralSettings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Utils
{
    public class Utils
    {
        private AppSettingsModel _AppConfig;
        public Utils(AppSettingsModel AppConfig)
        {
            _AppConfig = AppConfig;
        }

        public DataBaseConnections GetDataBase(string sDataBaseID)
        {
            DataBaseConnections? sConection = (from v in _AppConfig.DataBaseConnections
                                               where v.DataBaseID.ToUpper() == sDataBaseID.ToUpper()
                                               select v).FirstOrDefault();
            return sConection;
        }

        public ApiConnections GetApi(string sApiID)
        {
            ApiConnections? sConection = (from v in _AppConfig.ApiConnections
                                          where v.ApiID.ToUpper() == sApiID.ToUpper()
                                          select v).FirstOrDefault();
            return sConection;
        }

        public string SerializarParaJson(object c)
        {
            string JSONFINAL = JsonConvert.SerializeObject(c);
            return JSONFINAL;
        }

        public T DeserializarParaJson<T>(string Json)
        {
            var JSONFINAL = JsonConvert.DeserializeObject<T>(Json);
            return JSONFINAL;
        }
    }
}
