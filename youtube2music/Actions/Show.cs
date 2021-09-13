using System;
using System.Windows.Forms;

namespace youtube2music.Actions
{
    /// <summary>
    /// Show notice, question or error as MessageBox.
    /// </summary>
    public static class Show
    {
        /// <summary>
        /// Show MessageBox
        /// </summary>
        /// <param name="title">MessageBox title</param>
        /// <param name="text">MessageBox text</param>
        /// <param name="buttons">MessageBox buttons</param>
        /// <param name="icon">MessageBox icon</param>
        /// <returns>DialogResult from MessageBox</returns>
        private static DialogResult MBox(string title, string text, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(text, title, buttons, icon);
        }

        /// <summary>
        /// Show MessageBox with error.
        /// </summary>
        /// <param name="title">Error title</param>
        /// <param name="text1">Error text on first line</param>
        /// <param name="text2">Error text on second line</param>
        /// <param name="text3">Error text on third line</param>
        public static void Error(string title, string text1, string text2 = null, string text3 = null)
        {
            string text = text1;
            if (!String.IsNullOrEmpty(text2)) text += Environment.NewLine + text2;
            if (!String.IsNullOrEmpty(text3)) text += Environment.NewLine + text3;
            MBox(title, text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        /// <summary>
        /// Show MessageBox with question.
        /// </summary>
        /// <param name="title">Question title</param>
        /// <param name="text1">Question text on first line</param>
        /// <param name="text2">Question text on second line</param>
        /// <param name="text3">Question text on third line</param>
        /// <param name="buttons">MessageBox button</param>
        /// <returns>DialogResult from MessageBox</returns>
        public static DialogResult Question(string title, MessageBoxButtons buttons, string text1, string text2 = null, string text3 = null)
        {
            string text = text1;
            if (!String.IsNullOrEmpty(text2)) text += Environment.NewLine + text2;
            if (!String.IsNullOrEmpty(text3)) text += Environment.NewLine + text3;
            return MBox(title, text, buttons, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Show MessageBox with notice.
        /// </summary>
        /// <param name="title">Notice title</param>
        /// <param name="text1">Notice text on first line</param>
        /// <param name="text2">Notice text on second line</param>
        /// <param name="text3">Notice text on third line</param>
        public static void Notice(string title, string text1, string text2 = null, string text3 = null)
        {
            string text = text1;
            if (!String.IsNullOrEmpty(text2)) text += Environment.NewLine + text2;
            if (!String.IsNullOrEmpty(text3)) text += Environment.NewLine + text3;
            MBox(title, text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // TODO show operation
        /*public static void Operation(StatusStrip status,string text, string title = null)
        {
            if (!String.IsNullOrEmpty(title)) text = title.ToUpper() + ": " + text;

            StatusStrip.Invoke(new Action(() =>
            {
                labelOperace.Text = text;
            }));
        }*/
    }
}