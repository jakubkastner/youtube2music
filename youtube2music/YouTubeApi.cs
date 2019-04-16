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
                var snippet = response.Items[0].Snippet;
                // video existuje
                if (!String.IsNullOrEmpty(snippet.ChannelId))
                {
                    // získám původní název, kanál, popis, datum publikování
                    noveVideo.NazevPuvodni = snippet.Title;
                    noveVideo.Kanal = new Kanal(snippet.ChannelId, snippet.ChannelTitle);
                    noveVideo.Popis = snippet.Description;
                    noveVideo.Publikovano = snippet.PublishedAt.Value;
                    return;
                }
            }
            // video neexistuje nebo nastala chyba
            noveVideo.Chyba = "Video neexistuje";
        }

        // získá seznam uživatelovo playlistů
        internal static void ZiskejPlaylistyUzivatele(string uzivatelovoID)
        {
            var channelsListRequest = sluzbaYoutube.Playlists.List("snippet,contentDetails");
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
            var pozadavek = sluzbaYoutube.Playlists.List("snippet");
            pozadavek.Id = playlistID;
            var channelsListResponse = pozadavek.Execute();
            foreach (var list in channelsListResponse.Items)
            {
                return list.Snippet.Title;
            }
            return playlistID;
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
