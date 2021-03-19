using System;

namespace Task_2_1
{
    public class TheString
    {
        private char[] chars;
        private int length;
        public int Length
        {
            get
            {
                return length;
            }
            private set
            {
                length = value;
            }
        }
        public TheString(char[] _chars)
        {
            Length = _chars.Length;
            chars = new char[Length];
            _chars.CopyTo(chars, 0);
        }
        public char this[int index]
        {
            get
            {
                return chars[index];
            }
        }
        public bool Compare(TheString s)
        {
            if (Length != s.Length) return false;

            for (int i = 0; i < Length; i++)
            {
                if (chars[i] != s[i]) return false;
            }

            return true;
        }
        public int IndexOf(char c)
        {
            for (int i = 0; i < Length; i++)
            {
                if (chars[i] == c) return i;
            }
            return -1;
        }
        public TheString Concat(params TheString[] args)
        {
            int argsLength = Length;
            for (int i = 0; i < args.Length; i++)
            {
                argsLength += args[i].Length;
            }

            char[] c = new char[argsLength];
            int _argsLength = Length;
            chars.CopyTo(c, 0);

            for (int i = 0; i < args.Length; i++)
            {
                char[] _chars = args[i].ToCharArray();
                _chars.CopyTo(c, _argsLength);
                _argsLength += _chars.Length;
            }

            return new TheString(c);
        }
        public TheString Repeat(int count)
        {
            char[] _chars = new char[Length * count];

            for (int i = 0; i < count; i++)
            {
                chars.CopyTo(_chars, Length * i);
            }

            return new TheString(_chars);
        }
        public TheString Reverse()
        {
            char[] _chars = new char[Length];
            chars.CopyTo(_chars, 0);
            Array.Reverse(_chars);
            return new TheString(_chars);
        }
        public char[] ToCharArray()
        {
            char[] OutChars = new char[Length];
            chars.CopyTo(OutChars, 0);
            return OutChars;
        }
        public override string ToString()
        {
            return new string(chars);
        }
    }
}