using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youtube2music.App
{
    public class Programs
    {
        private string HistoryFilePath;
        public List<string> History { private set; get; }

        private string currentPath;

        private string CurrentPath
        {
            set
            {
                string newPath = value;
                AddToHistory(newPath);
                currentPath = newPath;
            }
            get
            {
                return currentPath;
            }
        }

        public Programs(string historyFileName)
        {
            // TODO show error
            if (String.IsNullOrEmpty(historyFileName)) return;
            if (Directories.Data == null) return;

            // full path
            string historyFilePath = Path.Combine(Directories.Data, historyFileName);
            HistoryFilePath = historyFilePath;

            GetHistoryFromFile();
        }

        private void GetHistoryFromFile()
        {
            History = FD.Files.Read(HistoryFilePath, true) ?? new List<string>();
        }

        private void AddToHistory(string newPath)
        {
            if (History.Contains(newPath)) return;
            History.Add(newPath);
        }
    }
}
