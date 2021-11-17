using System;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Type your text here:");
            char[] a = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] b = { 'z', 'y', 'x', 'w', 'v', 'u', 't', 's', 'r', 'q', 'p', 'o', 'n', 'm', 'l', 'k', 'j', 'i', 'h', 'g', 'f', 'e', 'd', 'c', 'b', 'a' };
            char[] a1 = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] b1 = { 'Z', 'Y', 'X', 'W', 'V', 'U', 'T', 'S', 'R', 'Q', 'P', 'O', 'N', 'M', 'L', 'K', 'J', 'I', 'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A' };

            string plain_text = Console.ReadLine();
            string cipher = "";
            char[] c = plain_text.ToCharArray();

            for (int i = 0; i < c.Length; i++)
                
                {for (int j = 0; j < a.Length; j++)
                {
                    if (c[i] == ' ') { cipher += c[i]; }
                    else
                    {
                        if (c[i] == b[j])
                        {
                            cipher += a[j];
                        }
                        else
                        {
                            if (c[i] == b1[j])
                            {
                                cipher += a1[j];
                            }
                        }
                    }
                }
                }
                    
                    Console.WriteLine(" Atbash encrypted text:{0}", cipher);
            
            //cesar cipher

            Console.WriteLine(" Cezar key");
            int k = Convert.ToInt32(Console.ReadLine());
            string cesar_cipher = "";
            int n = a.Length;
            for (int i = 0; i < c.Length; i++)

            {
                for (int j = 0; j < a.Length; j++)
                {
                    if (c[i] == ' ') { cesar_cipher += c[i]; }
                    else {
                        if (c[i] == a[j]) { cesar_cipher += a[(j + k) % n]; }
                        else { if (c[i] == a1[j]) { cesar_cipher += a1[(j  + k) % n]; } }
                    
                    }
                }
            }
            Console.WriteLine(" Cesar encrypted text:{0}", cesar_cipher);
            
            // cipher perestanovok
            string ciipher = "";
            
            /*
            int jol;
            if (Math.Sqrt(c.Length) % 1 == 0)
            {
                jol = Convert.ToInt32(Math.Sqrt(c.Length));

            }
            else { jol = Convert.ToInt32(Math.Sqrt(c.Length)); }
            Console.WriteLine("matrica jol sany:{0}",jol);
            //Convert.ToInt32(Console.ReadLine());
            //Console.Write("matrica bagan sany");
            int bagan = jol;//Convert.ToInt32(Console.ReadLine());
            char[,] p = new char[jol, bagan];
            int g = 0;
            for (int i = 0; i < jol; i++)
            {
                for (int j = 0; j < bagan; j++)
                {
                    if ((g + j+1) <= c.Length)
                    {
                        p[i, j] = c[g + j];ciipher += p[j,i];
                    }
                }g += jol;

            }
            for (int i = 0; i < jol; i++)
            {
                for (int j = 0; j < bagan; j++)
                {
                    if ((g + j + 1)  <= c.Length)
                    {
                        p[j,i] = p[i,j];
                        
                    }
                }
                g += jol;

            }

            Console.WriteLine(" Perestanovka encrypted text:{0}", ciipher);
            Console.WriteLine("length text:{0}", c.Length);*/
        }
    }
}

