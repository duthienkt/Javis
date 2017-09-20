using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Javis
{
    class VoiceService
    {
        protected String uniform_input;
        private string[] inputs;
        protected int uniform_item, uniform_rate, uniform_volume;
        public String script
        {
            get
            {
                return "Dim message, sapi\nmessage=\"" + uniform_input + "\"\nSet sapi = CreateObject(\"sapi.spvoice\")\n" +
                "Set sapi.Voice = sapi.GetVoices.Item(" + uniform_item.ToString() + ")\nsapi.Rate = " + uniform_rate.ToString() +
                "\nsapi.volume = " + uniform_volume.ToString() + "\n" +
                "sapi.Speak message";
            }
        }
        public VoiceService()
        {
            setInput("Hello, Did you forget something?").setRate(0).setVoiceItem(1).setVolume(100);
        }
        public VoiceService setInput(string x)
        {

            inputs = x.Split(new char[] {'\n'});
            return this;
        }
        public VoiceService run(string code)
        {
            foreach (var input in inputs)
            {

                uniform_input = input;
                System.IO.File.WriteAllText(code + ".vbs", script);
                Process scriptProc = new Process();
                scriptProc.StartInfo.FileName = @"cscript";
                //scriptProc.StartInfo.WorkingDirectory = @"./"; //<---very important 
                scriptProc.StartInfo.Arguments = "//B //Nologo " + code + ".vbs";
                scriptProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                scriptProc.Start();
                scriptProc.WaitForExit();
                scriptProc.Close();
            }
            return this;
        }

        public VoiceService setRate(int x)
        {
            if (x < 0 || x > 10) return this;
            uniform_rate = x;
            return this;
        }
        public VoiceService setVoiceItem(int x)
        {
            if (x < 0 || x > 2) return this;
            uniform_item = x;
            return this;
        }

        public VoiceService setVolume(int x)
        {
            if (x < 0 || x > 100) return this;
            uniform_volume = x;
            return this;
        }
    }
}
