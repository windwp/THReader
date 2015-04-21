using System;
using Windows.System;
using Windows.UI.Xaml;
using TungHoanhReader.Common;

namespace TungHoanhReader.Controls
{
    public sealed partial class InputBox
    {
        private readonly DialogHelper _service = new DialogHelper { AnimationType = DialogHelper.AnimationTypes.Fast };

        public InputBox()
        {
            InitializeComponent();
            ContentGrid.Opacity = 0;

            _service.Child = this;
            _service.Opened += (sender, args) => ShowStoryboard.Begin();
#if WINDOWS_PHONE_APP
            _service.BackKeyPressed += (sender, args) => { args.Handled = true; HideStoryboard.Begin(); };
#endif
            ShowStoryboard.Completed += (sender, o) => InputTextBox.Focus(FocusState.Programmatic);
            HideStoryboard.Completed += (sender, o) => _service.Hide();

            InputTextBox.KeyDown += (sender, args) => { if (args.Key != VirtualKey.Enter) return; OkOnClick(null, null); };
        }

        public void Show()
        {
            _service.Show();
        }

        public string Title { get { return TitleBlock.Text; } set { TitleBlock.Text = value; } }
        public string Message { get { return MessageBlock.Text; } set { MessageBlock.Text = value; } }
        public int MaxLength { get { return InputTextBox.MaxLength; } set { InputTextBox.MaxLength = value; } }

        public event EventHandler<string> OkClick;

        private void OnOkClick(string e)
        {
            var handler = OkClick;
            if (handler != null) handler(this, e);
        }

        private  void OkOnClick(object sender, RoutedEventArgs e)
        {
            HideStoryboard.Begin();
            OnOkClick(InputTextBox.Text.Trim());
        }

        private void CancelOnClick(object sender, RoutedEventArgs e)
        {
            HideStoryboard.Begin();
        }
    }
}
