using System;
using System.Linq;
using System.Reflection.Emit;

namespace Task_3_3
{
    static class StringExtension
    {
        public static CharExtension.LanguageType GetLanguageType(this string str)
        {
            // Способ 2?

            // string[] words = str.ToWordsArray();

            // if (words.All(word => word.All(c => c >= 'A' && c <= 'z'))) { return LanguageType.ENGLISH; }
            // else if (words.All(word => word.All(c => c >= 'А' && c <= 'я'))) { return LanguageType.RUSSIAN; }
            // else if (words.All(word => word.All(c => char.IsNumber(c)))) { return LanguageType.NUMBER; }
            // else { return LanguageType.MIXED; }

            CharExtension.LanguageType type = str.First(c => !char.IsPunctuation(c) && c != ' ').GetLanguageType();

            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsPunctuation(str[i]) && str[i] != ' ')
                {
                    CharExtension.LanguageType _type = str[i].GetLanguageType();

                    if (type != _type)
                    {
                        type = CharExtension.LanguageType.MIXED;
                    }
                }
            }

            return type;
        }

        public static string[] ToWordsArray(this string str)
        {
            return new string(str.Where(c => !char.IsPunctuation(c)).ToArray()).Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public static class CharExtension
    {
        private static char _ruStart = 'А';
        private static char _ruEnd = 'я';
        private static char _enStart = 'A';
        private static char _enEnd = 'z';

        public enum LanguageType
        {
            ENGLISH,
            RUSSIAN,
            NUMBER,
            MIXED,
        }

        public static LanguageType GetLanguageType(this char _char)
        {
            if (_char >= _enStart && _char <= _enEnd) { return LanguageType.ENGLISH; }
            else if (_char >= _ruStart && _char <= _ruEnd) { return LanguageType.RUSSIAN; }
            else if (Char.IsNumber(_char)) { return LanguageType.NUMBER; }
            else { return LanguageType.MIXED; }
        }
    }
}