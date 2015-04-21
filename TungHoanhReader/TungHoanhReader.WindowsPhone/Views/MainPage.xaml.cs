using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Practices.Prism.Mvvm;
using TungHoanhReader.Common;
using TungHoanhReader.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TungHoanhReader.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : IView
    {

        private SlideModel slideModel;
        double initialPosition;
        bool _viewMoved = false;
        private ObservableCollection<Item> ocTeams;
        public MainPage()
        {
            this.InitializeComponent();
            InitLeftSideScreen();
            SetListTruyen();


        }

        private void InitLeftSideScreen()
        {
            var model = this.DataContext as MainPageViewModel;
            //var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            if (model != null)
            {
                model.ScreenWidth = (int)Window.Current.Bounds.Width;
                model.ScreenHeight = (int)(Window.Current.Bounds.Height - 30);
            }
        }

        private const int MovePosX = -330;

        private void OpenClose_Left(object sender, RoutedEventArgs e)
        {

            var left = Canvas.GetLeft(LayoutRoot);
            if (left > -100)
            {
                MoveViewWindow(MovePosX);
            }
            else
            {
                MoveViewWindow(0);
            }
        }

        private void canvas_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _viewMoved = false;
            initialPosition = Canvas.GetLeft(LayoutRoot);
        }

        void MoveViewWindow(double left)
        {
            _viewMoved = true;
            //if (left == -420)
            //    ApplicationBar.IsVisible = true;
            //else
            //    ApplicationBar.IsVisible = false;
            ((Storyboard)canvas.Resources["moveAnimation"]).SkipToFill();
            ((DoubleAnimation)((Storyboard)canvas.Resources["moveAnimation"]).Children[0]).To = left;
            ((Storyboard)canvas.Resources["moveAnimation"]).Begin();
        }





        private void ItemListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = this.DataContext as MainPageViewModel;
            if (model != null && listviewTheLoai.SelectedItem != null)
            {
                var truyen = listviewTheLoai.SelectedItem as TagTruyenModel;
                if (truyen != null) model.TheLoaiDangXem = truyen.Value;
                OpenClose_Left(null, null);
                model.LoadingTheLoaiCommand.Execute();
            }
        }

        private void SiteTruyen_SelectonChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = this.DataContext as MainPageViewModel;
            if (model != null && listviewSiteTruyen.SelectedItem != null)
            {
                var truyen = listviewSiteTruyen.SelectedItem as SiteTruyenVM;
                OpenClose_Left(null, null);
                SetListTruyen();
                listviewTheLoai.SelectedItem = null;
                listviewTheLoai.Margin = new Thickness(0, 100, 0, 0);

                if (truyen != null) model.ChangeSiteTruyenCommand.Execute(truyen);
            }

        }

        private void SetListTruyen()
        {
            if (listviewSiteTruyen.Items != null)
            {
                listviewSiteTruyen.Items.Clear();
                listviewSiteTruyen.Items.Add(new SiteTruyenVM()
                {
                    Display = "truyenconvert.com",
                    Site = Wrapper.SiteTruyen.TruyenConvert
                });
                listviewSiteTruyen.Items.Add(new SiteTruyenVM() { Display = "lsb-thuquan.eu", Site = Wrapper.SiteTruyen.LuongSonBac });
                listviewSiteTruyen.Items.Add(new SiteTruyenVM() { Display = "tunghoanh.com", Site = Wrapper.SiteTruyen.TungHoanh });
            }
        }



        private void ListView_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            //listviewTruyen.sc();
        }

        private void ListViewRefresh_OnLoad(object sender, RoutedEventArgs e)
        {
            if (sender is ListView)
            {
                var model = this.DataContext as MainPageViewModel;
                if (model != null)
                {
                    var listview = sender as ListView;
                    listview.SetScrollPosition(null, model.ListViewScrollValue);
                    listview.SelectedItem = null;
                }
            }

        }


    }
}