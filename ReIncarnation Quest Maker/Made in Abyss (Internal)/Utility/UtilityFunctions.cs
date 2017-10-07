using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Parser;

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
                    case QuestFormatParser.SafetyKey:
                    case '"':
                    //case '{':
                    //case '}':
                        ReturnString += QuestFormatParser.SafetyKey;
                        break;
                }
                ReturnString += StringToConvert[x];
            }
            return ReturnString;
        }
    }
}
