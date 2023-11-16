using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DruidsCornerApp.Utils
{
    /// <summary>
    /// Observable collection that accepts batch inputs an only raises a single 
    /// event when the whole addition is over.
    /// Same goes for deletion.
    /// Heavily inspired from : https://stackoverflow.com/a/8607159/8716917
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BatchObservableCollection<T> : ObservableCollection<T>
    {
        public void InsertRange(IEnumerable<T> items)
        {
            CheckReentrancy();
            foreach (var item in items)
            {
                Items.Add(item);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

}
