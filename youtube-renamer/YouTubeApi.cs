using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;

namespace hudba
{
    public class YouTubeApi
    {
        private static YouTubeService sluzbaYoutube = Auth();

        private static YouTubeService Auth()
        {
            UserCredential povereni;
            // podhodí soubor s client id api mého programu
            using (var ctecka = new FileStream("youtube_client_secret.json", FileMode.Open, FileAccess.Read))
            {
                povereni = GoogleWebAuthorizationBroker.AuthorizeAsync
                (
                    GoogleClientSecrets.Load(ctecka).Secrets,
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore("YouTubeAPI")
                ).Result;
            }

            var sluzba = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = povereni,
                ApplicationName = "YouTubeAPI"
            });
            return sluzba;
        }

        public static void ZiskejInfoVidea(Video vid)
        {
            var pozadavek = sluzbaYoutube.Videos.List("snippet");
            pozadavek.Id = vid.id;

            var response = pozadavek.Execute();
            if (response.Items.Count > 0)
            {
                vid.nazevPuvodni = response.Items[0].Snippet.Title;
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

        /// <summary>
        /// Získá seznam ID videí z playlistu.
        /// </summary>
        /// <param name="playlistID">ID playlistu</param>
        /// <returns>Vrátí seznam ID videí z playlistu.</returns>
        internal static List<string> ZiskejIDVidei(string playlistID)
        {
            var pozadavek = sluzbaYoutube.PlaylistItems.List("contentDetails");
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
