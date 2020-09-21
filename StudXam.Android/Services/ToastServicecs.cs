using Android.App;
using Android.Widget;
using StudXam.Droid.Services;
using StudXam.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(ToastService))]

namespace StudXam.Droid.Services
{
    public class ToastService : IToast
    {
        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}