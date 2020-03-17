using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    static class Vvod
    {
       static public int[] Eng(string st) {
            int[] vo = new int[st.Length];
            for (int i = 0; i < st.Length; i++)
            {
               
                switch (Convert.ToString(st[i]).ToLower())
                {
                    case "a": vo[i] = 30; break;
                    case "b": vo[i] = 48; break;
                    case "c": vo[i] = 46; break;
                    case "d": vo[i] = 32; break;
                    case "e": vo[i] = 18; break;
                    case "f": vo[i] = 33; break;
                    case "g": vo[i] = 34; break;
                    case "h": vo[i] = 35; break;
                    case "i": vo[i] = 23; break;
                    case "j": vo[i] = 36; break;
                    case "k": vo[i] = 37; break;
                    case "l": vo[i] = 38; break;
                    case "m": vo[i] = 50; break;
                    case "n": vo[i] = 49; break;
                    case "o": vo[i] = 24; break;
                    case "p": vo[i] = 25; break;
                    case "q": vo[i] = 16; break;
                    case "r": vo[i] = 19; break;
                    case "s": vo[i] = 31; break;
                    case "t": vo[i] = 20; break;
                    case "u": vo[i] = 22; break;
                    case "v": vo[i] = 47; break;
                    case "w": vo[i] = 17; break;
                    case "x": vo[i] = 45; break;
                    case "y": vo[i] = 21; break;
                    case "z": vo[i] = 44; break;

                }

            }
            return vo;
        } 

    }
}
