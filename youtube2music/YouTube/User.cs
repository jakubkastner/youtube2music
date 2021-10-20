
using System;
using System.Linq;
using System.Windows.Forms;

namespace youtube2music.YouTube
{
    /// <summary>
    /// YouTube user class.
    /// </summary>
    public class User
    {
        /// <summary>
        /// User ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User channel name.
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// Link to user channel.
        /// </summary>
        public string ChannelUrl
        {
            get
            {
                if (String.IsNullOrEmpty(Id)) return null;
                return "https://youtube.com/channel/" + Id;
            }
        }

        public User(string id, string channelName)
        {
            this.Id = id;
            this.ChannelName = channelName;
        }

        /// <summary>
        /// Login user into the program.
        /// </summary>
        public bool Login()
        {
            Id = null;
            ChannelName = null;

            // call YouTube API
            User newUser = Api.GetUser();

            if (String.IsNullOrEmpty(ChannelName)) return false;
            if (String.IsNullOrEmpty(Id)) return false;

            App.Init.FormList.menuNastaveniUzivatel.Text = "Logged in user '" + ChannelName + "'";
            App.Init.FormList.menuNastaveniUzivatel.ToolTipText = "View YouTube channel";
            App.Init.FormList.menuNastaveniUzivatel.Enabled = true;
            App.Init.FormList.menuNastaveniUzivatelOdhlasit.Text = "Logout from YouTube";
            App.Init.FormList.menuNastaveniUzivatelOdhlasit.Enabled = true;

            return true;
        }
    }
}
