﻿
using TungHoanhReader.Common;

namespace TungHoanhReader.Controls
{
    public sealed partial class Toast
    {
        private readonly DialogHelper _service = new DialogHelper { AnimationType = DialogHelper.AnimationTypes.Fast };

        public Toast()
        {
            InitializeComponent();
            _service.Child = this;
            _service.Opened += (sender, args) => PopupMessageStoryboard.Begin();
            PopupMessageStoryboard.Completed += (sender, o) => _service.Hide();
        }

        public void Show()
        {
            _service.Show();
        }

        public string Message
        {
            get { return MessageText.Text; }
            set { MessageText.Text = value; }
        }

        public static void Show(string message)
        {
            var toast = new Toast { Message = message };
            toast.Show();
        }
    }
}
