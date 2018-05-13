namespace Hms.UI.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    public class PropertyChangedListener<T> : INotifyPropertyChanged
        where T : INotifyPropertyChanged
    {
        private readonly ObservableCollection<T> collection;

        private readonly string propertyName;

        private readonly Dictionary<T, int> items = new Dictionary<T, int>(new ObjectIdentityComparer());

        public PropertyChangedListener(ObservableCollection<T> collection, string propertyName = "")
        {
            this.collection = collection;
            this.propertyName = propertyName ?? string.Empty;
            this.AddRange(collection);
            CollectionChangedEventManager.AddHandler(collection, this.CollectionChanged);
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.AddRange(e.NewItems.Cast<T>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    this.RemoveRange(e.OldItems.Cast<T>());
                    break;
                case NotifyCollectionChangedAction.Replace:
                    this.AddRange(e.NewItems.Cast<T>());
                    this.RemoveRange(e.OldItems.Cast<T>());
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.Reset();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void AddRange(IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                if (this.items.ContainsKey(item))
                {
                    this.items[item]++;
                }
                else
                {
                    this.items.Add(item, 1);
                    PropertyChangedEventManager.AddHandler(item, this.ChildPropertyChanged, this.propertyName);
                }
            }
        }

        private void RemoveRange(IEnumerable<T> oldItems)
        {
            foreach (T item in oldItems)
            {
                this.items[item]--;

                if (this.items[item] == 0)
                {
                    this.items.Remove(item);
                    PropertyChangedEventManager.RemoveHandler(item, this.ChildPropertyChanged, this.propertyName);
                }
            }
        }

        private void Reset()
        {
            foreach (T item in this.items.Keys.ToList())
            {
                PropertyChangedEventManager.RemoveHandler(item, this.ChildPropertyChanged, this.propertyName);
                this.items.Remove(item);
            }

            this.AddRange(this.collection);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            handler?.Invoke(sender, e);
        }

        private class ObjectIdentityComparer : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return ReferenceEquals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            }
        }
    }
}
