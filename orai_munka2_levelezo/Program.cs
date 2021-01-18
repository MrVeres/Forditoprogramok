using System;
using System.Collections.Generic;
/*
 * +12, -3, 2 , 000, 001
 * D = {0, 2, .... ,9}
 * (+|-|e)D+
 * "(+,-)[0-9]+"
 * 𝛿 = {delta((q0, +) -> q1), delta((q0, -) -> q1), (q1, N, q2), (q2, N, q2)}
 * 
 */

//2020.11.21
//regexautomation
namespace orai_munka2_levelezo
{
    class Program
    {
        class automata
        {
            string input;
            int i = 0;
            string state;
            Dictionary<string, string> D = new Dictionary<string, string>();
            public automata(string input)
            {
                this.input = input;
                this.state = "q0";
                D.Add("q0-", "q1");
                D.Add("q0+", "q1");
                D.Add("q0d", "q2");
                D.Add("q1d", "q2");
                D.Add("q2d", "q2");
            }
            public void main()
            {
                while (i < input.Length && state != "error")
                {
                    state = delta(state, input[i]);
                    i++;
                }
                if (state == "error")
                {
                    Console.WriteLine("{0} kifejezés helytelen, a hiba poziciója {1}," +
                        " a hibás karakter {2}",
                        input, i, input[i-1]);
                }
                else
                {
                    Console.WriteLine("A input kifejezés helyes: {0}", input);
                }
            }

            private string delta(string state, char v)
            {
                string elemek = state + convert(v); //+, -,  0-9 -> d
                // q0- , q0+ , q0d , q1d ...  egyéb -> error
                if (D.ContainsKey(elemek))
                    return D[elemek];
                else
                    return "error";
            }

            private char convert(char v)
            {
                if (Char.IsDigit(v))
                {
                    return 'd';
                }
                return v;
            }
        }
        static void Main(string[] args)
        {
            automata automata = new automata("+20");
            automata.main();
        }
    }
}