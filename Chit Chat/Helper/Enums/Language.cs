using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChat.Helper.Enums
{
    public enum Language
    {
        [Description("ar-IL")]
        Arabic,
        [Description("en")]
        English,
        [Description("fi-FI")]
        Finnish,
        [Description("he-IL")]
        Hebrew,
        [Description("ja-JP")]
        Japanese
        
    }
}
