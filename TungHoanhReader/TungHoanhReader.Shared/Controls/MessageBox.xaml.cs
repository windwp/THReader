using System;
using Windows.UI.Xaml;
using TungHoanhReader.Common;

namespace TungHoanhReader.Controls
{
    public sealed partial class MessageBox
    {
        readonly DialogHelper _service = new DialogHelper { AnimationType = DialogHelper.AnimationTypes.Fast };
        public MessageBox()
        {
            InitializeComponent();

            ContentGrid.Opacity = 0;
            HideStoryboard.Completed += (sender, o) => _service.Hide();

            _service.Child = this;
            _service.Opened += (sender, args) => ShowStoryboard.Begin();
            _service.Closed += Closed;
#if WINDOWS_PHONE_APP
            _service.BackKeyPressed += (sender, args) =>
            {
                args.Handled = true;
                HideStoryboard.Begin();
            };
#endif
        }

        public string Title
        {
            get { return TitleBlock.Text; }
            set { if (value != null) TitleBlock.Text = value; }
        }
        public string Message
        {
            get { return MessageBlock.Text; }
            set { if (value != null) MessageBlock.Text = value; }
        }

        public string PositiveButtonTitle
        {
            get { return PositiveButton.Content as string; }
            set { PositiveButton.Content = value; }
        }

        public string NegativeButtonTitle
        {
            get { return NegativeButton.Content as string; }
            set { NegativeButton.Content = value; }
        }

        public void Show()
        {
            _service.Show();
        }

        public event EventHandler Closed;

        public event EventHandler<object> OkClick;

        private void OnOkClick(object e)
        {
            var handler = OkClick;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<object> CancelClick;

        private void OnCancelClick(object e)
        {
            var handler = CancelClick;
            if (handler != null) handler(this, e);
        }

        private void OkOnClick(object sender, RoutedEventArgs e)
        {
            HideStoryboard.Begin();
            OnOkClick(e);
        }

        private void CancelOnClick(object sender, RoutedEventArgs e)
        {
            HideStoryboard.Begin();
            OnCancelClick(e);
        }
    }
}
