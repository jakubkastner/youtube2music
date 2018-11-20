using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;

using System.IO;
using System.Threading;

using System.Windows.Forms;

namespace hudba
{
    public class YouTubeApi
    {
        private static YouTubeService ytService = Auth();

        private static YouTubeService Auth()
        {
            UserCredential creds;
            using (var strm = new FileStream("youtube_client_secret.json", FileMode.Open, FileAccess.Read))
            {
                creds = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(strm).Secrets,
                                                                    new[] { YouTubeService.Scope.YoutubeReadonly },
                                                                    "user",
                                                                    CancellationToken.None,
                                                                    new FileDataStore("YouTubeAPI")
                                                                    ).Result;
            }

            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = creds,
                ApplicationName = "YouTubeAPI"
            });
            return service;
        }

        public static void GetVideoInfo(Video vid)
        {
            var videoRequest = ytService.Videos.List("snippet");
            videoRequest.Id = vid.id;

            var response = videoRequest.Execute();
            if (response.Items.Count > 0)
            {
                vid.puvodniNazev = response.Items[0].Snippet.Title;
                vid.kanalID = response.Items[0].Snippet.ChannelId;
                vid.kanal = response.Items[0].Snippet.ChannelTitle;
                vid.poznamka = response.Items[0].Snippet.Description;
                vid.publikovano = response.Items[0].Snippet.PublishedAt.Value;
            }
            else
            {
                vid.chyba = "video neexistuje";
            }
        }




        // získá id videí z playlistu
        internal static List<string> ZiskejIDVidei(string playlistID)
        {
            var pozadavek = ytService.PlaylistItems.List("contentDetails");
            pozadavek.PlaylistId = playlistID;

            List<string> videaID = new List<string>();
            string dalsiStrana = "";
            while (dalsiStrana != null)
            {
                pozadavek.PageToken = dalsiStrana;
                try
                {
                    var odpoved = pozadavek.Execute();
                    foreach (var item in odpoved.Items)
                    {
                        videaID.Add(item.ContentDetails.VideoId);
                    }
                    dalsiStrana = odpoved.NextPageToken;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return videaID;
        }



        internal static List<Video> GetPlaylistInfo(string playlistID)
        {
            var pozadavek = ytService.PlaylistItems.List("contentDetails");            
            pozadavek.PlaylistId = playlistID;

            List<Video> videa = new List<Video>();
            string nextPage = "";
            while (nextPage != null)
            {
                pozadavek.PageToken = nextPage;
                try
                {
                    var response = pozadavek.Execute();
                    foreach (var item in response.Items)
                    {
                        //FormSeznam frm = new FormSeznam();
                        //frm.labelInfo.Text = "Přidávám:" + Environment.NewLine + item.ContentDetails.VideoId;
                        //frm.Text = "Přidávám:" + Environment.NewLine + item.ContentDetails.VideoId;
                        //MessageBox.Show("Přidávám:" + Environment.NewLine + item.ContentDetails.VideoId);
                        
                        videa.Add(new Video(item.ContentDetails.VideoId));
                    }
                    nextPage = response.NextPageToken;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return videa;
        }
    }
}
