using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace TungHoanhReader.Services.DialogService
{
    public interface IDialogService
    {
        void Show(string content, string title = default(string));
        void Show(string content, string title, params global::Windows.UI.Popups.UICommand[] commands);

        void ShowS(string content, string title = default(string));
        void ShowInput(string content, string title, EventHandler<string> okHandler);
    }
}
