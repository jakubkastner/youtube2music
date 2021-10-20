using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youtube2music.YouTube
{
    public class Init
    {
        public static User LoggedInUser { get; private set; }

        /// <summary>
        /// Login user into the program.
        /// </summary>
        public static bool Login()
        {
            // call YouTube API
            User newUser = Api.GetUser();

            if (String.IsNullOrEmpty(newUser.ChannelName)) return false;
            if (String.IsNullOrEmpty(newUser.Id)) return false;

            LoggedInUser = newUser;

            App.Init.FormList.menuNastaveniUzivatel.Text = "Logged in user '" + LoggedInUser.ChannelName + "'";
            App.Init.FormList.menuNastaveniUzivatel.ToolTipText = "View YouTube channel";
            App.Init.FormList.menuNastaveniUzivatel.Enabled = true;
            App.Init.FormList.menuNastaveniUzivatelOdhlasit.Text = "Logout from YouTube";
            App.Init.FormList.menuNastaveniUzivatelOdhlasit.Enabled = true;

            return true;
        }
    }
}
