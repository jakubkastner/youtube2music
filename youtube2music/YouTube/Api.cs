using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;

namespace youtube2music.YouTube
{
    /// <summary>
    /// YouTube API
    /// </summary>
    public class Api
    {
        /// <summary>
        /// YouTube service.
        /// </summary>
        private static readonly YouTubeService serviceYT = Auth();

        /// <summary>
        /// Authorize YouTube user.
        /// </summary>
        /// <returns>YouTubeService</returns>
        private static YouTubeService Auth()
        {
            UserCredential credential;
            // init json for api
            string apiInit = @"{""installed"":{""client_id"":""806982074560-2h1sptig11iq8hrl4ink1jetllv1vlm9.apps.googleusercontent.com"",""project_id"":""youtube-renamer"",""auth_uri"":""https://accounts.google.com/o/oauth2/auth"",""token_uri"":""https://accounts.google.com/o/oauth2/token"",""auth_provider_x509_cert_url"":""https://www.googleapis.com/oauth2/v1/certs"",""client_secret"":""FDJhnbA5J2BLPpSxVD01vz4K"",""redirect_uris"":[""urn:ietf:wg:oauth:2.0:oob"",""http://localhost""]}}";

            // read anit api json
            using (var reader = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(apiInit)))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync
                (
                    GoogleClientSecrets.FromStream(reader).Secrets,
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(Path.Combine(App.Paths.Directories.Data))
                ).Result;
            }

            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "youtube2music"
            });
            return service;
        }

        /// <summary>
        /// Get currently logged in user id and name of he channel.
        /// </summary>
        internal static void GetUser()
        {
            // YouTube API request
            var request = serviceYT.Channels.List("brandingSettings");
            request.Mine = true;

            // YouTube API result
            var result = request.Execute();
            if (result.Items == null) return;
            if (result.Items.Count == 0) return;

            var channel = result.Items[0];
            if (String.IsNullOrEmpty(channel.BrandingSettings.Channel.Title)) return;

            User.Id = channel.Id;
            User.ChannelName = channel.BrandingSettings.Channel.Title;
        }



        /// <summary>
        /// Získá z YouTube API informace o videu (název, kanál, popis, publikováno).
        /// </summary>
        /// <param name="noveVideo">Nové video k získání informací z YouTube API.</param>
        public static void ZiskejInfoVidea(Video noveVideo)
        {
            var pozadavek = serviceYT.Videos.List("snippet");
            pozadavek.Id = noveVideo.ID;

            var response = pozadavek.Execute();
            if (response.Items.Count > 0)
            {
                var snippet = response.Items[0].Snippet;
                // video existuje
                if (!String.IsNullOrEmpty(snippet.ChannelId))
                {
                    // získám původní název, kanál, popis, datum publikování
                    noveVideo.NazevPuvodni = snippet.Title;
                    noveVideo.Kanal = new Kanal(snippet.ChannelId, snippet.ChannelTitle);
                    noveVideo.Popis = snippet.Description;
                    //noveVideo.Publikovano = DateTime.ParseExact(snippet.PublishedAt, "yyyy-MM-ddTHH:mm:ssZ", null);
                    noveVideo.Publikovano = (DateTime)snippet.PublishedAt;
                    return;
                }
            }
            // video neexistuje nebo nastala chyba
            noveVideo.Chyba = "Video neexistuje";
        }

        // získá seznam uživatelovo playlistů
        internal static void ZiskejPlaylistyUzivatele(string uzivatelovoID)
        {
            var channelsListRequest = serviceYT.Playlists.List("snippet,contentDetails");
            channelsListRequest.ChannelId = uzivatelovoID;
            channelsListRequest.MaxResults = 50;
            var channelsListResponse = channelsListRequest.Execute();
            foreach (var list in channelsListResponse.Items)
            {
                Console.WriteLine(list.Id);
                Console.WriteLine(list.Snippet.Title);
            }
        }

        internal static string ZiskejNazevPlaylistu(string playlistID)
        {
            var pozadavek = serviceYT.Playlists.List("snippet");
            pozadavek.Id = playlistID;
            var channelsListResponse = pozadavek.Execute();
            foreach (var list in channelsListResponse.Items)
            {
                return list.Snippet.Title;
            }
            return playlistID;
        }

        internal static void ZiskejNazevUzivatele()
        {
            var pozadavek = serviceYT.Channels.List("brandingSettings");
            pozadavek.Mine = true;
            var odpoved = pozadavek.Execute();
            if (odpoved.Items == null)
            {
                return;
            }
            if (odpoved.Items.Count == 0)
            {
                return;
            }
            var kanal = odpoved.Items[0];
            if (kanal.BrandingSettings.Channel.Title == null)
            {
                return;
            }
            YouTube.User.Id = kanal.Id;
            YouTube.User.ChannelName = kanal.BrandingSettings.Channel.Title;
        }

        /// <summary>
        /// Získá seznam ID videí z playlistu.
        /// </summary>
        /// <param name="playlistID">ID playlistu</param>
        /// <returns>Vrátí seznam ID videí z playlistu.</returns>
        internal static List<string> ZiskejIDVidei(string playlistID)
        {
            var pozadavek = serviceYT.PlaylistItems.List("contentDetails");
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
    }
}
