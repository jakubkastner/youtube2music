using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;

namespace youtube_renamer
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

        public static void ZiskejInfoVideaStare(VideoStare vid, string playlist)
        {
            var pozadavek = sluzbaYoutube.Videos.List("snippet");
            pozadavek.Id = vid.id;

            var response = pozadavek.Execute();
            if (response.Items.Count > 0)
            {
                vid.nazevPuvodni = response.Items[0].Snippet.Title;
                vid.kanalID = response.Items[0].Snippet.ChannelId;
                vid.kanal = response.Items[0].Snippet.ChannelTitle;
                vid.popis = response.Items[0].Snippet.Description;
                vid.publikovano = response.Items[0].Snippet.PublishedAt.Value;
                vid.playlist = playlist;
            }
            else
            {
                vid.chyba = "video neexistuje";
            }
        }
        /// <summary>
        /// Získá z YouTube API informace o videu (název, kanál, popis, publikováno).
        /// </summary>
        /// <param name="noveVideo">Nové video k získání informací z YouTube API.</param>
        public static void ZiskejInfoVidea(Video noveVideo)
        {
            var pozadavek = sluzbaYoutube.Videos.List("snippet");
            pozadavek.Id = noveVideo.ID;

            var response = pozadavek.Execute();
            if (response.Items.Count > 0)
            {
                noveVideo.Nazev.Puvodni = response.Items[0].Snippet.Title;
                noveVideo.Kanal = new VideoKanal(response.Items[0].Snippet.ChannelId, response.Items[0].Snippet.ChannelTitle);
                noveVideo.Popis = response.Items[0].Snippet.Description;
                noveVideo.Publikovano = response.Items[0].Snippet.PublishedAt.Value;
            }
            else
            {
                noveVideo.Chyba = "Video neexistuje";
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
