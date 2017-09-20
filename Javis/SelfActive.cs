using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace Javis
{
    class WhereAreYou : Phrase
    {
        private static string[] phrases = {"Where are you?", "What is your folder?", "Can I see you?" }; 
        public double match(string sinp, Core core)
        {
            var res = -1.0;
            foreach(var p in phrases)
            {
                var e = Core.phraseLike(p, sinp);
                if (e > res)
                    res = e;
            }
            return res;
        }

        public void request(string sinp, Core core, ResponseCallback callback)
        {
            Process.Start(@".");
        }
    }
    class Run : Phrase
    {
       
        public double match(string sinp, Core core)
        {
            var res = -1.0;
            if (sinp.StartsWith(">"))  res = 1000;
            return res;
        }

        public void request(string sinp, Core core, ResponseCallback callback)
        {

            sinp = sinp.Trim().Substring(1).Trim();
            string cmd = "";
            for (int i = 0; i< sinp.Length;++i)
            {
                if (sinp[i] == ' ' || sinp[i] == '\n') break;
                cmd += sinp[i];
            }
            string arg = "";
            if (cmd.Length < sinp.Length)
                arg = sinp.Substring(cmd.Length).Trim();
            //MessageBox.Show(arg);
            try
            {
                if (arg.Length > 0)
                    Process.Start(cmd, arg);
                else
                    Process.Start(cmd);
            }
            catch(Exception e)
            {
                callback(e.ToString());
            }
        }
    }
}
