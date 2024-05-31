using Domain.Models;
namespace Domain.Interfaces
{
    public interface IUserService
    {
        DefaultResponseModel Create(CreateUserRequestModel request);
        DefaultResponseModel Alter(AlterUserRequestModel request);
    }
}
