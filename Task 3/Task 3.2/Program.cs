using System;
using System.Collections;
using System.Collections.Generic;

namespace Task_3_2
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicArray<int> list = new DynamicArray<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            list.AddRange(new int[] { 8, 8, 8, 8, 8, 8, 8 });

            list.Add(13);
            list.Add(14);

            list.Remove(5);
            list.Insert(99, 4);

            Console.WriteLine(String.Join(", ", list.ToArray()));
            Console.WriteLine(list.Length);
            Console.WriteLine(list.Capacity);

            list.SetCapasity(2);

            Console.WriteLine(list.Capacity);

            DynamicArray<int> clonedList = (DynamicArray<int>)list.Clone();

            Console.WriteLine(String.Join(", ", clonedList.ToArray()));

            CycledDynamicArray<string> cycledList = new CycledDynamicArray<string>(new string[] { "Vova", "Artem", "Daniil", "Vlad" });

            // foreach (string item in cycledList)
            // {
            //     Console.Write(item + " ");
            // }
        }
    }

    class DynamicArray<T> : IEnumerable<T>, ICloneable
    {
        private T[] _data;

        public int Length { get; private set; } = 0;
        public int Capacity { get; private set; } = 8;
        public T this[int index]
        {
            get { return _data[GetIndex(index)]; }
            set { _data[GetIndex(index)] = value; }
        }

        public DynamicArray()
        {
            _data = new T[Capacity];
        }
        public DynamicArray(IEnumerable<T> collection)
        {
            int collectionCount = 0;

            foreach (T item in collection) { collectionCount++; }

            int factor = GetCapacityFactor(collectionCount);

            Capacity = Capacity * (int)Math.Pow(2, factor);

            _data = new T[Capacity];

            foreach (T item in collection) { _data[Length++] = item; };
        }

        public void Add(T item)
        {
            if (Length + 1 > Capacity)
            {
                SetCapasity(Capacity * 2);
            }

            Length++;

            _data[Length - 1] = item;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            int collectionCount = 0;

            foreach (T item in collection) { collectionCount++; }

            if (Capacity < collectionCount + Length)
            {
                int factor = GetCapacityFactor(collectionCount + Length);

                SetCapasity(Capacity * (int)Math.Pow(2, factor));
            }

            foreach (T item in collection)
            {
                _data[Length++] = item;
            }
        }

        public bool Remove(T item)
        {
            int index = Array.IndexOf<T>(_data, item);

            if (index >= 0)
            {
                for (int i = index; i < Length; i++)
                {
                    _data[i] = _data[i + 1];
                }

                Length--;

                return true;
            }

            return false;
        }

        public bool Insert(T item, int index)
        {
            if (index > -1 && index < Length)
            {
                if (Length + 1 > Capacity)
                {
                    SetCapasity(Capacity * 2);
                }

                T currentItem = _data[index];
                T nextItem;

                for (int i = index; i < Length; i++)
                {
                    nextItem = _data[i + 1];
                    _data[i + 1] = currentItem;
                    currentItem = nextItem;
                }

                _data[index] = item;

                Length++;

                return true;
            }
            else { throw new ArgumentOutOfRangeException(); }

            // return false;
        }

        public void SetCapasity(int newCapasity)
        {
            if (newCapasity <= 0) { throw new ArgumentException("New capacity size must be greater than 0"); }

            T[] tmp = new T[newCapasity];

            if (newCapasity < Capacity && Length > newCapasity)
            {
                Length = newCapasity;
            }

            Array.Copy(_data, 0, tmp, 0, Length);

            Capacity = newCapasity;
            _data = tmp;
        }

        public T[] ToArray()
        {
            T[] tmp = new T[Length];

            Array.Copy(_data, tmp, Length);

            return tmp;
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return _data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();

        public object Clone()
        {
            return new DynamicArray<T>(this);
        }

        private int GetCapacityFactor(int size)
        {
            int count = 0;

            while (Capacity * 2 * count < size)
            {
                count++;
            }

            return count;
        }

        private int GetIndex(int index)
        {
            if (-index > -(Length + 1) && index < Length)
            {
                int _index = index;

                if (index < 0) { _index = Length + index; }

                return _index;
            }
            else { throw new ArgumentOutOfRangeException(); }
        }
    }

    class CycledDynamicArray<T> : DynamicArray<T>
    {
        public CycledDynamicArray() : base() { }

        public CycledDynamicArray(IEnumerable<T> collection) : base(collection) { }

        public new virtual IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];

                if (i + 1 == Length) { i = 0; }
            }
        }
    }
}
