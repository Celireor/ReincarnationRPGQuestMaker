using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace ReIncarnation_Quest_Maker.Made_in_Abyss.Utility
{
    public static class LinqExtensions
    {
        public static T Swap <T> (this IList<T> list, int indexA, int indexB)
        {
            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
            return list[indexA]; //the original has been moved already
        }

        public static void AssignIndexValuesToListItems<T>(this IList<T> list, Action<T, int> actiontotake) {
            for (int x = 0; x < list.Count; x++) {
                actiontotake(list[x], x);
            }
        }

        public static void UpdatePossibleValues(this ComboBox ThisBox, List<string> Values)
        {
            ThisBox.Items.Clear();
            Values.Sort();
            for (int x = 0; x < Values.Count; x++)
            {
                ThisBox.Items.Add(Values[x]);
            }
        }
    }
}
