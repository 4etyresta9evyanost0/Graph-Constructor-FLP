using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Graph_Constructor_FLP
{
    public static class Extensions
    {
        public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        public static DependencyObject GetChild(DependencyObject reference, int childIndex, int count)
        {
            DependencyObject dpObj = reference;
            for (int i = 0; i < count; i++) {
                dpObj = VisualTreeHelper.GetChild(dpObj, childIndex);
            }
            return dpObj;
        }

        public static DependencyObject GetChild(DependencyObject reference, params int[] count)
        {
            DependencyObject dpObj = reference;
            for (int i = 0; i < count.Length; i++)
            {
                dpObj = VisualTreeHelper.GetChild(dpObj, count[i]);
            }
            return dpObj;
        }
    }
}
