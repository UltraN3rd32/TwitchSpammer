using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Media;

namespace TwitchSpammer
{
    public partial class Form1 : Form
    {
        private TwitchIRC irc;

        private static Boolean spamming;

        private static Random r = new Random();

        public Form1()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();
            axWindowsMediaPlayer1.Visible = false;
            Properties.Settings.Default.Save();
            progressBar1.Maximum = (int)numericUpDown1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 0 && textBox1.Text.Length > 0)
            {
                irc = new TwitchIRC(Properties.Settings.Default.username, textBox1.Text, "oauth:"+Properties.Settings.Default.oauth);
                irc.setMessage(richTextBox1.Text);
                spamming = !spamming;
                button1.Text = spamming ? "Stop spamming" : "Spam";
                if (!spamming)
                {
                    progressBar1.Value = 0;
                    button1.BackColor = Form1.DefaultBackColor;
                    button1.ForeColor = Color.Black;
                } 
            }
        }

        private static String randomize(String message)
        {
            int index = r.Next(0, message.Length-1);
            int attempts = 0;
            while (message[index] != ' ' && attempts < 30)
            {
                index = r.Next(0, message.Length-1);
                attempts += 1;
            }
            String s = "";
            if (attempts >= 30)
                return message + " " + r.Next(0, message.Length*42);

            for (int i = 0; i < index; i++)
                s += message[i];
            s += " ";
            for(int i = index; i < message.Length; i++)
                s += message[i];
            return s;
        }

        private static int n = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(irc != null && spamming == true)
            {
                if (progressBar1.Value + timer1.Interval <= progressBar1.Maximum)
                {
                    progressBar1.Value += timer1.Interval;
                }
                else {
                    if (irc.sendMessage())
                    {
                        String last = randomize(richTextBox1.Text);
                        irc.setMessage(last);
                        progressBar1.Value = 0;
                    } else
                    {
                        button1.PerformClick();
                        button1.PerformClick();
                    }
                }
            }
            var frequency = .2;
            var red = (int)(Math.Sin(frequency * n / 5) * 127 + 128);
            var green = (int)(Math.Sin(frequency * n / 5 + 2 * Math.PI / 3) * 127 + 128);
            var blue = (int)(Math.Sin(frequency * n / 5 + 4 * Math.PI / 3) * 127 + 128);
            var col = (255 << 24) | (red << 16) | (green << 8) | blue;

            linkLabel2.LinkColor = Color.FromArgb(col);

            n += 1;

            if(n > 1000)
            {
                n = 0;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            progressBar1.Maximum = (int)numericUpDown1.Value;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (spamming) button1.PerformClick();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (spamming) button1.PerformClick();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
        }

        private void tabPage2_Selected(Object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Text == "Config")
            {
                tabControl1.Height = 146;
                this.Height = 180;
                textBox2.Text = Properties.Settings.Default.username;
                textBox3.Text = Properties.Settings.Default.oauth;
                axWindowsMediaPlayer1.URL = "keygen.mp3";
                axWindowsMediaPlayer1.Ctlcontrols.play();
            } else
            {
                tabControl1.Height = 176;
                this.Height = 211;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.oauth = textBox3.Text;
            Properties.Settings.Default.Save();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://blueberrypancak.es");
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitchapps.com/tmi/");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.username = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
