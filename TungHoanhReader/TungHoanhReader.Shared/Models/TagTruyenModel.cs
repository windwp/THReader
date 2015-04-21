using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TungHoanhReader
{
    public class TagTruyenModel:INotifyPropertyChanged
    {
        public Wrapper.TagTruyen Value { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

       // [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
