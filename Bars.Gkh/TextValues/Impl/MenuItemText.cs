namespace Bars.Gkh.TextValues.Impl
{
    using System.Collections.Generic;

    using Bars.Gkh.TextValues;

    public class MenuItemText : IMenuItemText
    {
        private readonly Dictionary<string, string> itemTextDictionary = new Dictionary<string, string>();

        public string GetText(string menuItemText)
        {
            if (this.itemTextDictionary.ContainsKey(menuItemText))
            {
                return this.itemTextDictionary[menuItemText];
            }

            return menuItemText;
        }

        public void Override(string baseTextValue, string newTextValue)
        {
            this.itemTextDictionary[baseTextValue] = newTextValue;
        }
    }
}