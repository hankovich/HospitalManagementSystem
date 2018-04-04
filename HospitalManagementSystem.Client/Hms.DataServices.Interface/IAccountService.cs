namespace Hms.DataServices.Interface
{
    using System.Threading.Tasks;

    public interface IAccountService
    {
        Task LoginAsync(string login, string password);

        Task RegisterAsync(string login, string password);

        Task LogoutAsync();
    }
}
