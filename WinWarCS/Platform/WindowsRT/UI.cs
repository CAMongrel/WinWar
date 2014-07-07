using System;
using System.Threading.Tasks;

namespace WinWarCS.Platform
{
    public static class UI
    {
        public static async Task<bool> ShowMessageDialog(string message)
        {
            Windows.UI.Popups.MessageDialog dlg = new Windows.UI.Popups.MessageDialog(message, "WinWarCS - WarCraft for Windows Modern UI");
            await dlg.ShowAsync();
            return true;
        }
    }
}

