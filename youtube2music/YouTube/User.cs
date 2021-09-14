
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
        /// Main FormList with list of videos.
        /// </summary>
        private static readonly FormSeznam formList = Application.OpenForms.OfType<FormSeznam>().FirstOrDefault();

        /// <summary>
        /// User ID.
        /// </summary>
        public static string ID { get; set; }

        /// <summary>
        /// User channel name.
        /// </summary>
        public static string ChannelName { get; set; }

        /// <summary>
        /// Link to user channel.
        /// </summary>
        public static string ChannelUrl
        {
            get
            {
                if (String.IsNullOrEmpty(ID)) return null;
                return "https://youtube.com/channel/" + ID;
            }
        }
        
        /// <summary>
        /// Login user into the program.
        /// </summary>
        public static bool Login()
        {
            YouTubeApi.ZiskejNazevUzivatele();
            if (ChannelName == null)
            {
                return false;
            }
            formList.menuNastaveniUzivatel.Text = "Logged in user '" + ChannelName + "'";
            formList.menuNastaveniUzivatel.ToolTipText = "View YouTube channel";
            formList.menuNastaveniUzivatel.Enabled = true;
            formList.menuNastaveniUzivatelOdhlasit.Text = "Logout from YouTube";
            formList.menuNastaveniUzivatelOdhlasit.Enabled = true;
            return true;
        }
    }
}
