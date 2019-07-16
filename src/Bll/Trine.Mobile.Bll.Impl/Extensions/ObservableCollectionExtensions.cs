using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Trine.Mobile.Bll.Impl.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                if (condition(collection[i]))
                {
                    collection.RemoveAt(i);
                }
            }
        }
    }

    public class RangeEnabledObservableCollection<T> : ObservableCollection<T>
    {
        public void InsertRange(int index, IEnumerable<T> items)
        {
            CheckReentrancy();
            foreach (var item in items)
                Items.Insert(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
