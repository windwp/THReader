namespace TungHoanhReader.Services.DialogService
{
    public interface IDialogService
    {
        void Show(string content, string title = default(string));
        void Show(string content, string title, params global::Windows.UI.Popups.UICommand[] commands);
    }
}
