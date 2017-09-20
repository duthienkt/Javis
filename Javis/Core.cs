using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Json;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace Javis
{
    class Core
    {
        private static Random rand = new Random();
        private static Regex sep = new Regex("[,.? !]+");
        private static Regex spa = new Regex("[ ,]+");

        public static int random()
        {
            return rand.Next();
        }
        public static T random<T>(T [] arr){
            return arr[rand.Next() % arr.Length];
        }

        public static T random<T>(List<T> list)
        {
            return list[rand.Next() % list.Count];
        }

        public static double wordLike(String a, String b)
        {
            int m = a.Length;
            int n = b.Length;
            double[,] Q = new double[m + 1, n + 1];
            for (var i = 0; i <= m; ++i)
                for (var j = 0; j <= n; ++j)
                    Q[i, j] = 0.0;
            for (var i = 0; i < m; ++i)
                for (var j = 0; j < n; ++j)
                {
                    Q[i + 1, j + 1] = Math.Max(Q[i + 1, j], Q[i, j + 1]);
                    if (a[i] == b[j])
                        Q[i + 1, j + 1] = Math.Max(Q[i + 1, j + 1], Q[i, j] + 1);
                }
            return Q[m, n] / Math.Max(Math.Max(m, n), 1);
        }
        public static String [] split(String s)
        {
            String res = s.ToLower();
            res = sep.Replace(res, " ");
            res = res.Trim();
            return spa.Split(res);
        }
        public static double phraseLike(String a, String b)
        {
           
            var sq1 = split(a);
            var sq2 = split(b);
            var m = sq1.Length;
            var n = sq2.Length;
        
            var Q = new double[m+1, n+1];
            for (var i = 0; i <= m; ++i)
                for (var j = 0; j<=n; ++j)
                Q[i, j] = 0.0;
            for (var i = 0; i < m; ++i)
                for (var j = 0; j < n; ++j)
                {
                    Q[i + 1, j + 1] = Math.Max(Q[i + 1, j], Q[i, j + 1]);
                        Q[i + 1, j + 1] = Math.Max(Q[i + 1, j + 1], Q[i, j] + wordLike(sq1[i], sq2[j]));
                }
            return Q[m, n] / Math.Max(Math.Max(m, n), 1);
        }

        public const string DATA_BASE_JSON = "database.json";
        public Dictionary<String, Phrase> phrases {
            get;
        }
        public Core()
        {
            database = JsonValue.Parse(System.IO.File.ReadAllText(DATA_BASE_JSON));
            phrases = new Dictionary<string, Phrase>();
        }

      
        public JsonValue database
        {
            get;
        }

        public void saveDatabase()
        {
          
            System.IO.File.WriteAllText(DATA_BASE_JSON,database.ToString());
        }
        public static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }
        public void request(string sinp, ResponseCallback callback)
        {
            Phrase matcher =null;
            var best = -1.0;
            var e = -1.0;

            foreach (var m in phrases)
            {
                e = m.Value.match(sinp, this);
                if (e> best)
                {
                    best = e;
                    matcher = m.Value;
                }
            }
            if(matcher!= null)
            matcher.request(sinp, this, callback);
        }
    }

    public delegate void ResponseCallback(string result);

    interface Phrase
    {
        double match(string sinp, Core core);
        void request(string sinp, Core core, ResponseCallback callback);
    }

}
