using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace ReIncarnation_Quest_Maker.Made_In_Abyss_Internal.Utility
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

        public static void UpdatePossibleValues(this ComboBox ThisBox, List<string> Values, bool IgnoreNonFittingItems = false)
        {
            ThisBox.Items.Clear();
            Values.Sort();
            Values.Distinct().ToList().ForEach(obj => {

                if (!IgnoreNonFittingItems || obj.Contains(ThisBox.Text))
                {
                    ThisBox.Items.Add(obj);
                }
            });
        }
    }
}
