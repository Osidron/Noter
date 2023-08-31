using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noter
{
    internal class Convert
    {
        public static string noteToString(note_data note)
        {
            return $"{note.title}\n{note.text}";
        }
        public static int stringToInt(string str)
        {
            Boolean isParsed = int.TryParse(str, out int i);
            if (isParsed)
            {
                return i;
            }
            return -1;

        }
    }
}
