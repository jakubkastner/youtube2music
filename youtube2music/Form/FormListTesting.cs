using System;
using System.Windows.Forms;

namespace youtube2music
{
    public partial class FormSeznam : Form {

        /**** TESTOVÁNÍ ****/

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1Zxe-AFgLWX4esfmBVOm1-2"; //2018.11 us & others
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1b6j6ZP8glz9s8iqHi1v-KD!"; // 2017.04 us & others
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/watch?v=TwOpW6dP7FU"; // video
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1aDbeKJy6MFAbihUXQPmzVZ"; // 2018.11 cz
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1Z07yHjt9YtHRcK6M7tVSC6"; // 2018.10 us & others        
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1bLKMsXFkozb0tJNg-t3g4m"; // ZKOUŠKA
        }
        private void albumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://music.youtube.com/playlist?list=OLAK5uy_nefv5WJefq7o6l9fMnjYmvHN-1HRWqKPQ"; //ALBUM KILLY
        }
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(App.Directories.LibraryMp3);
        }
    }
}
