using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility
{
    public static class UtilityFunctions
    {
        public static void Swap2Variables<T>(ref T first, ref T second)
        {
            T temp = first;
            first = second;
            second = temp;
        }
        public static void Swap2Variables<T>(T first, T second)
        {
            T temp = first;
            first = second;
            second = temp;
        }

        public static string SafeQuotationMarks(string StringToConvert) {
            string ReturnString = "";
            for (int x = 0; x < StringToConvert.Length; x++) {
                switch (StringToConvert[x])
                {
                    case '\\':
                    case '"':
                    case '{':
                    case '}':
                        ReturnString += '\\';
                        break;
                }
                ReturnString += StringToConvert[x];
            }
            return ReturnString;
        }
    }
}
