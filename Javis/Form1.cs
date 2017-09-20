using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Json;


namespace Javis
{
    public partial class Javis : Form
    {

        Core ai;
        public Javis()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = messBox;
            ai = new Core();
            ai.phrases.Add("static_memo", new StaticMemo());
            ai.phrases.Add("learning", new Learning());
            ai.phrases.Add("current_folder", new WhereAreYou());
            ai.phrases.Add("vocab", new VocabTest());
            ai.phrases.Add("run", new Run());
            //ai.phrases.Add("power_active", new PowerActive());

        }

        private void Form1_MouseHover(object sender, EventArgs e)
        {
            this.Opacity = 0.9;
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            this.Opacity = 0.6;
        }

        private void messBox_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }
        string lastTest = "";
        int deep = 0;
        private void onReciveMessage(string res)
        {
            if (res.StartsWith("@")&& deep< 40)
            {
                ++deep;
                    ai.request(res.Substring(1), onReciveMessage);
            }
            else
            {
                lastTest = res;
                conversationBox.SelectionColor = Color.Red;
                conversationBox.SelectionFont = new Font(conversationBox.SelectionFont, FontStyle.Bold);
                conversationBox.AppendText("Javis : ");
                conversationBox.SelectionColor = Color.Black;
                conversationBox.SelectionFont = new Font(conversationBox.SelectionFont, FontStyle.Regular);
                conversationBox.AppendText(res + "\n");
            }
        }
        
        private void messBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Shift && e.KeyCode == Keys.Enter)
            {
                if (messBox.Text.Length > 0)
                {
                    string mess = messBox.Text;
                    conversationBox.Select(conversationBox.TextLength, 0);
                    conversationBox.SelectionColor = Color.Blue;
                    conversationBox.SelectionFont = new Font(conversationBox.SelectionFont, FontStyle.Bold);
                    conversationBox.AppendText("Me : ");
                    conversationBox.SelectionColor = Color.Black;
                    conversationBox.SelectionFont = new Font(conversationBox.SelectionFont, FontStyle.Regular);

                    conversationBox.AppendText(mess + "\n");
                    lastTest = mess;
                    messBox.Text = "";
                    deep = 0;
                    ai.request(mess, onReciveMessage);

                }
                else
                    new VoiceService().setInput(lastTest).run("speak");
            }
        }

        private void messBox_TextChanged(object sender, EventArgs e)
        {
            if (messBox.Text.Trim().Length == 0) messBox.Text = "";
        }

        private void conversationBox_TextChanged(object sender, EventArgs e)
        {
            conversationBox.ScrollToCaret();
        }
    }
}
