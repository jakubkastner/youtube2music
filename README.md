# archive2music

### 🇨🇿 Please note
This program is currently in development and only in the Czech language!

### ‼ Upozornění
Program byl primárně vytvořen pro moje osobní použití. Aktuálně existují [omezení](#-aktuální-omezení), která budou možná v budoucích verzích odstraněna.

1. [ℹ About](#ℹ-about)
2. [⏬ Download](#-download)
3. [🔍 How program works](#-how-program-works)
4. [⚠ Necessary programs for youtube2music functionality](#-necessary-programs-for-youtube2music-functionality)
5. [➕ Optional programs](#-optional-programs)

# ℹ About
Windows application for better organization of music downloaded using [youtube-dl](https://github.com/ytdl-org/youtube-dl).

# ⏬ Download
The program is free to download at this link:
* [Download msi installer here](https://github.com/jakubkastner/youtube2music/releases/download/0.0.1/youtube2music_installer.msi)

# 🔍 How program works
* Add a link to a YouTube video or playlist or YouTube Music Album.
* The program retrieves information about the videos (channel name, video name, video release date).
* youtube2music will try to get the artist, song title and featured artists from the video information.
* If an artist has already been found in your library, the program will automatically retrieve the genre, folder and new file name.
* You can then edit this information as you wish.
* If a song has already been found in your music library, the program will notify you.
* If you add an album, the program will try to find album information (artist, release year, playlist, cover) using Deezer API. 
* Once you have filled in all the necessary information about the videos, you can download them via youtube-dl (youtube2music does not download any videos - everything is furnished using this external program).
* The program then downloads the mp3 videos to your mp3 music library and uses ffmpeg to convert them to opus and save them to your opus music library.
* After downloading, you can still check the files in the mp3tag and add additional information if necessary.

# ⚠ Necessary programs for youtube2music functionality
These programs must be installed and the path to the executable exe file set in the program settings.
* [youtube-dl](https://youtube-dl.org/) 
* [ffmpeg](https://www.ffmpeg.org/) 

# ➕ Optional programs
These programs can be installed and the path to the executable exe file set in the program settings. Used to subsequently open downloaded files and add tags to them.
* [mp3tag](https://www.mp3tag.de/)

