using System.Reflection;

namespace Domain.Interfaces
{
    public interface IStarterService
    {
        Assembly GetAssembly();
        List<TableInformationModel> GetTables();
    }
}
