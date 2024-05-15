using Data.API;
using Domain.Interfaces;

namespace Service.Services
{
    public class BaseService : IBaseService
    {
        private ApiDefaultAccess _api;
        public BaseService(ApiDefaultAccess api)
        {
            _api = api;
        }
    }
}
