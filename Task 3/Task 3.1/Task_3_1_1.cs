using System;
using System.Linq;
using System.Collections.Generic;

namespace Task_3_1
{
    class WeakestLink
    {
        private List<int> _data = null;
        private List<int> _willDelete = null;
        private int _n = 0;
        public WeakestLink(int n)
        {
            if (n < 2) { throw new ArgumentException("Argument must be greater than 2"); }
            _n = n;
            Init();
        }

        public void DeleteEvery(int n)
        {
            if (n > _n) { throw new ArgumentException("Argument must be less than list size"); }
            int current = 0;

            while (_data.Count >= n)
            {
                for (int i = 0; i < _data.Count; i++)
                {
                    current++;

                    if (current == n)
                    {
                        _willDelete.Add(_data[i]);
                        current = 0;
                    }
                }

                Console.WriteLine($"deleted -> {String.Join(' ', _willDelete.ToArray())}");

                Clear();

                Console.WriteLine($"stay -> {String.Join(' ', _data.ToArray())}");
            }
        }

        private void Init()
        {
            _data = new List<int>(Enumerable.Range(1, _n));
            _willDelete = new List<int>();
        }
        private void Clear()
        {
            _data.RemoveAll(item => _willDelete.Contains(item));
            _willDelete.Clear();
        }
    }
}