using System.Threading.Tasks;
using Xamarin.Forms;

namespace StudXam.Interfaces
{
    public interface INavigationService
    {
        Task PushAsync(Page page);

        Task<Page> PopAsync();

        Task<bool> DisplayAlert(string title, string message, string ok, string cancel);

        Task DisplayAlert(string title, string message, string ok);
    }
}