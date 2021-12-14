using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChitChat.Helper.Language
{
    public interface ILanguage
    {
        void ChangeLanguage();
        Array ChangeBoundEnumLanguage(Array values)
        {

            List<string> newValues = new List<string>();
            foreach (string enumName in values)
            {
                var newValue = Application.Current.Resources[$"{enumName}{this.GetType().Name}"];
                newValues.Add((string)newValue);
            }
            return newValues.ToArray();
        }
    }
}
