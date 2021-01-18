using System;
using System.Text.RegularExpressions;


namespace munka2_lev_recautomaton
{
    class Program
    {
        // Rekurziv leszállási módszer
        //2020.11.21
        class RecursivePRG
        {
            int i = 0;
            string input;

            void S()
            {
                E();
                elfogad('#');
            }
            void E()
            {
                T();
                Ev();
            }
            void Ev()
            {
                if (input[i] == '+')
                {
                    elfogad('+');
                    T();
                    Ev();
                }
            }
            void T()
            {
                F();
                Tv();
            }
            void Tv()
            {
                if (input[i] == '*')
                {
                    elfogad('*');
                    F();
                    Tv();
                }
            }
            void F()
            {
                if (input[i] == '(')
                {
                    elfogad('(');
                    E();
                    elfogad(')');
                }
                else
                {
                    elfogad('i');
                }
            }
            public RecursivePRG(string input)
            {
                this.input = $"{simple(input)}#";
                S();
                Console.WriteLine("Az elemzés lefutott");
            }
            private string simple(string input)
            {
                Console.WriteLine("Eredeti input {0}", input);
                string v = Regex.Replace(input, "[0-9]+", "i");
                Console.WriteLine("Egyszerűsitett imput: {0}", v + '#');
                return v;
            }
            public void elfogad(char c)
            {
                if (input[i] != c)
                {
                    Console.WriteLine("Hibás a kifejezés: {0}, helytelen karakter: {1}", input, input[i]);
                }
                i++;
            }

        }

        static void Main(string[] args)
        {
            RecursivePRG recursivePRG = new RecursivePRG("(1+alma)*3");
        }
    }
}