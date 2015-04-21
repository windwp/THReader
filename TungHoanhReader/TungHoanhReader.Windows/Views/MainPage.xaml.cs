using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TungHoanhReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page,IView
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
