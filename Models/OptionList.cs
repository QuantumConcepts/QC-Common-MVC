using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuantumConcepts.Common.Mvc.Models
{
    public class OptionList<T> : IList<T>
        where T : ListItem
    {
        public List<T> Options { get; set; }

        public int Count { get { return this.Options.Count; } }

        public T this[int index] { get { return this.Options[index]; } set { this.Options[index] = value; } }

        public OptionList()
        {
            this.Options = new List<T>();
        }

        public OptionList(IEnumerable<T> collection)
        {
            this.Options = new List<T>(collection);
        }

        public int IndexOf(T item)
        {
            return this.Options.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.Options.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this.Options.RemoveAt(index);
        }

        public void Add(T item)
        {
            this.Options.Add(item);
        }

        public void Clear()
        {
            this.Options.Clear();
        }

        public bool Contains(T item)
        {
            return this.Options.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Options.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly { get { return false; } }

        public bool Remove(T item)
        {
            return this.Options.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Options.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Options.GetEnumerator();
        }
    }
}