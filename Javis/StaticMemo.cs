using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Json;
namespace Javis
{
    class StaticMemo : Phrase
    {
        string cache = "";
        public double match(string sinp, Core core)
        {
            var m = -1.0;
            var e = 0.0;
            ArrayList res = new ArrayList();

            var phrases = core.database["phrases"];
            
            for (var i = 0; i < phrases.Count; ++i)
            {
                var p = phrases[i];
                
                e = Core.phraseLike(sinp, (string)p["sinp"]);
             
                if (e > m)
                {
                    m = e;
                    res.Clear();
                    res.Add((string)p["resp"]);
                }
                else
                {
                    if (e == m)
                    {
                        res.Add((string)p["resp"]);
                    }
                }
            }
            cache = (string)Core.random(res.ToArray());
            return m;
        }

        public void request(string simp, Core core, ResponseCallback callback)
        {
            callback(cache);
        }
    }

    class Learning : Phrase
    {
        private static string[] resps = {"Learnt!", "Yup", "Recorded"};
        public double match(string sinp, Core core)
        {
            if (sinp.StartsWith("#"))
            {
                sinp = sinp.Replace("#", "");
                var s = sinp.Split(new char[] {'\\'});
                if (s.Length != 2) return -1;
                s[0] = s[0].Trim();
                s[1] = s[1].Trim();
                //var json = "{\"sinp\": " + Core.ToLiteral(s[0]) + ",\n\"resp\": " + Core.ToLiteral(s[1]) + "}";
                //MessageBox.Show(json);

                var v = new JsonObject();
                v.Add("sinp", s[0]);
                v.Add("resp", s[1]);


                ((JsonArray)core.database["phrases"]).Add(v);
                core.saveDatabase();
                return 100;
            }
            return -1;
        }

        public void request(string simp, Core core, ResponseCallback callback)
        {
            callback(Core.random(resps));
        }
    }

    class VocabTest : Phrase
    {
        public class Test
        {
            public string question;
            public string[] answer;
            public int correct;

            public Test(String question, String[] answer, int correct)
            {
                this.question = question;
                this.answer = answer;
                this.correct = correct;

            }
            public override string ToString()
            {
                return question + "\n    A. " + answer[0] + "\n    B. " + answer[1] + "\n    C. " + answer[2] + "\n    D. " + answer[3] + "\n";
            }
        }


        public class TestSheet
        {
            public static TestSheet load(string path)
            {
                String[] t = System.IO.File.ReadAllLines(path);
                TestSheet sheet = new TestSheet();
                for (int i = 5; i < t.Length; i += 7)
                {

                    sheet.add(
                            new Test(t[i - 5], new String[] { t[i - 4], t[i - 3], t[i - 2], t[i - 1], }, Int32.Parse(t[i]))
                    );

                }
                return sheet;
            }

            private ArrayList data;

            private TestSheet()
            {
                data = new ArrayList();
            }

            public void add(Test test)
            {
                data.Add(test);
            }


            public override string ToString()
            {
                String res = "";
                for (int i = 0; i < data.Count; ++i)
                {
                    res += data[i].ToString() + "\n";
                }
                return res;
            }

            public Test get(int i)
            {
                return (Test)data[i];
            }

            public int size()
            {
                return data.Count;
            }
        }


        private static string[] phrases = { "TOIEC", "Vocabulary", "Test Vocabulary" };
        private static string[] phrases2 = { "exit", "quit", "tired" };
        private static string[] respcr = { "Nice!", "Good job.", "Cool!", "Correct!" };
        private static string[] respicr = { "Wrong.", "I don't think so.", "Incorrect", "Try again!" };

        bool hold = false;
        private int[] F;
        private int sF;
        private int point;
        private const string VOCAB_TXT = "vocab.txt";
        private const string VOCAB_SF_TXT = "vocab_sf.txt";

        private TestSheet sheet = null;
        private void saveData()
        {
            string s = "" +point;
            for (int i = 0; i < F.Length; ++i)
                s += " " + F[i];
            System.IO.File.WriteAllText(VOCAB_SF_TXT, s);
        }

        public void loadData()
        {
            if (sheet != null) return;
            if (System.IO.File.Exists(VOCAB_TXT))
                sheet = TestSheet.load(VOCAB_TXT);
            else
                return;

            string d = null;
            if (System.IO.File.Exists(VOCAB_SF_TXT))
                d = System.IO.File.ReadAllText(VOCAB_SF_TXT);
            F = new int[sheet.size()];
            sF = 0;
            point = 0;
            if (d == null)
            {
                point = 0;
                for (int i = 0; i < F.Length; ++i)
                {
                    sF += 10;
                    F[i] = 10;
                }
            }
            else
            {
                String[] t = d.Split(new char[] { ' ' });
                point = Int32.Parse(t[0]);
                for (int i = 0; i < F.Length; ++i)
                {
                    F[i] = Int32.Parse(t[i+1]);
                    sF += F[i];
                }
            }
            nextQuestion();
        }

        public double match(string sinp, Core core)
        {
            var res = -1.0;
            if (hold)
            {
                return 1000;
            }

            foreach (var p in phrases)
            {
                var e = Core.phraseLike(p, sinp);
                if (e > res)
                    res = e;
            }
            return res;
        }

        public void request(string sinp, Core core, ResponseCallback callback)
        {
            loadData();
            if (!hold)
            {
                nextQuestion();
                callback(currentTest.ToString());
                hold = true;
                return;
            }
               
           
            var res = -1.0;
            if (hold)
            {
                foreach (var p in phrases2)
                {
                    var e = Core.phraseLike(p, sinp);
                    if (e > res)
                        res = e;
                }
                if (res > 0.8)
                {
                    hold = false;
                    int pr = (int)(1000000 - ((double)F.Length / sF * 10) * 1000000);
                    if (pr < 0) pr = 0;
                    
                    callback("You completed  " + (pr / 10000.0) + "%, "+ point +" points");
                    saveData();
                    return;
                }
            }

            if (sheet == null)
                callback("I can find \"vocab.txt\"!");
            else
            {
                var r = -1;
                sinp = sinp.Trim().ToLower();
                if (sinp.Equals("a")) r = 0;
                if (sinp.Equals("b")) r = 1;
                if (sinp.Equals("c")) r = 2;
                if (sinp.Equals("d")) r = 3;
                if (select(r))
                {
                    nextQuestion();
                    callback(Core.random(respcr));
                    callback(currentTest.ToString());
                }
                    
                else
                    callback(Core.random(respicr));
            }
            //todo
        }

        Test currentTest;
        int currentTestId;
        Random rand = new Random();
        private void nextQuestion()
        {
            int s = Math.Abs(rand.Next()) % sF;
            int t = 0;
            currentTestId = 0;
            while (t < s)
            {
                t += F[currentTestId];
                ++currentTestId;
            }
            currentTest = sheet.get(currentTestId);
        }

        private bool select(int c)
        {
            bool res = false;
            //String mess = "Incorrect!";
            point -= 1;
            if (c == currentTest.correct)
            {
                if (F[currentTestId] > 1)
                {
                    F[currentTestId]--;
                    sF--;
                }
                //mess = "Correct n_n";
                res = true;
                point += 3;
            }
            
         
            return res;
        }
    }
}
