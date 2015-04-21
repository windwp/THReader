using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using TungHoanhReader.Controls;
#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
#endif

namespace TungHoanhReader.Services.DialogService
{
    public class DialogService : IDialogService
    {
        bool _open = false;
        //DialogHelper _helper=new DialogHelper(){AnimationType = DialogHelper.AnimationTypes.Fade};
        public DialogService()
        {
            
          
        }

        public async void Show(string content, string title = default(string))
        {
            while (_open) { await Task.Delay(1000); }
            _open = true;
            var dialog = new MessageDialog(content, title);
            await dialog.ShowAsync();
            _open = false;
        }


        public async void ShowS(string content, string title = default(string))
        {
            while (_open) { await Task.Delay(1000); }
            _open = true;
            var dialog = new MessageBox(){Message = content,Title = title};
            dialog.Show();
            _open = false;
        }
        public async void Show(string content, string title, params UICommand[] commands)
        {
            while (_open) { await Task.Delay(1000); }
            _open = true;
            var dialog = new MessageDialog(content, title);
            foreach (var item in commands)
                dialog.Commands.Add(item);
            await dialog.ShowAsync();
            _open = false;
        }

        public async void ShowInput(string content, string title, EventHandler<string> okHandler)
        {
            while (_open) { await Task.Delay(1000); }
            _open = true;
            var dialog = new InputBox(){Title = title,Message = content};
            dialog.OkClick += okHandler;
            dialog.Show();
            _open = false;
        }


        //pu

   
    }
    
}
