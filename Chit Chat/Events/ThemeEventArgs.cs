using ChitChat.Helper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Events
{
    public class ThemeEventArgs : EventArgs
    {
        public Theme? NewTheme { get; set; }
    }
}
