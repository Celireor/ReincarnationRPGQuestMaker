using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReIncarnation_Quest_Maker.Made_in_Abyss.Utility
{
    public static class UtilityFunctions
    {
        public static void Swap2Variables<T>(ref T first, ref T second) {
            T temp = first;
            first = second;
            second = temp;
        }
    }
}
