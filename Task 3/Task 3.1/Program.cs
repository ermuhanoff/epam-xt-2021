using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Task_3._1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task 3.1

            WeakestLink people = new WeakestLink(7);

            people.DeleteEvery(2);

            // people.DeleteEvery(3);

            // people.DeleteEvery(4);

            //Task 3.2

            TextAnalysis textAnalysis = new TextAnalysis();

            textAnalysis.Analyze("Jimmy was a bad man, because he writting ban word on walls");

            textAnalysis.Analyze("Jimmy was a jimmy, because he writting jimmy word on jimmy walls");

            textAnalysis.Analyze("Jimmy was a jimmy, because jimmy writting jimmy jimmy on jimmy jimmy");

            textAnalysis.Analyze("Jimmy jimmy jimmy jimmy, jimmy jimmy jimmy jimmy jimmy jimmy jimmy jimmy");

            textAnalysis.Analyze("Jimmy jimmy jimmy jimmy, danny danny danny danny, manny manny manny manny");

            textAnalysis.Analyze("Jimmy jimmy jimmy jimmy jimmy jimmy jimmy jimmy jimmy jimmy jimmy, manny");

            textAnalysis.Analyze("Jimmy danny manny");
        }
    }
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
            if (n < _n) { throw new ArgumentException("Argument must be greater than list size"); }
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

    class TextAnalysis
    {
        private Dictionary<string, int> _wordsDict;
        private float _uniqueThreshold = 0.15f;
        private float _dispThreshold = 0.7f;
        private int _wordThreshold = 4;
        private int _wordCount;
        private string _text;

        public TextAnalysis()
        {
            _wordsDict = new Dictionary<string, int>();
        }

        public void Analyze(string text)
        {
            _text = text;

            ConvertTextToDict();

            (double percentOfPopularWord, string mostPopularWord) = GetMostPopularWord();
            double disp = GetDispersion();
            double avgPercents = GetAveragePecrent();
            string analysisResult;

            if (disp == 0)
            {
                if (_wordCount < _wordThreshold)
                {
                    analysisResult = "Text too small.";
                }
                else if (percentOfPopularWord == 1)
                {
                    analysisResult = "Hmmm... The result is no very good...";
                }
                else if (percentOfPopularWord > _uniqueThreshold)
                {
                    analysisResult = "To few unique words.";
                }
                else
                {
                    analysisResult = "All word in text different. Well done.";
                }
            }
            else
            {
                if (disp > _dispThreshold)
                {
                    analysisResult = "Text is too monotonous. It could be better.";
                }
                else
                {
                    analysisResult = "Text is quite varied. Good job.";
                }
            }

            // log

            Console.WriteLine("Words statistics:");

            foreach (KeyValuePair<string, int> pair in _wordsDict)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }

            Console.WriteLine($"Most popular word '{mostPopularWord}', percent of this word: {ToFixedPercent(percentOfPopularWord)}%");
            Console.WriteLine($"Average percent of words in text: {ToFixedPercent(avgPercents)}%");
            Console.WriteLine($"Analyze result: {analysisResult}");

            _wordsDict.Clear();
        }

        private void ConvertTextToDict()
        {
            string[] words = new string(_text.Where(c => !char.IsPunctuation(c)).ToArray()).Split(' ', StringSplitOptions.RemoveEmptyEntries);

            _wordCount = words.Length;

            foreach (string word in words)
            {
                string loweredWord = word.ToLower();

                if (_wordsDict.ContainsKey(loweredWord))
                {
                    _wordsDict[loweredWord]++;
                }
                else
                {
                    _wordsDict.Add(loweredWord, 1);
                }
            }
        }

        private (double percent, string word) GetMostPopularWord()
        {
            int[] wordsCount = _wordsDict.Values.ToArray();
            int maxCount = wordsCount.Max();
            string mostPopularWord = _wordsDict.FirstOrDefault(item => item.Value == maxCount).Key;
            double percentOfPopularWord = (double)maxCount / _wordCount;

            return (percentOfPopularWord, mostPopularWord);
        }

        private double GetDispersion()
        {
            int[] wordsCount = _wordsDict.Values.ToArray();
            double avg = wordsCount.Average();
            double sum = 0;

            foreach (int count in wordsCount)
            {
                sum += Math.Pow(count - avg, 2);
            }

            return sum / _wordCount;
        }
        private double GetAveragePecrent()
        {
            int[] wordsCount = _wordsDict.Values.ToArray();
            double[] percents = wordsCount.Select(item => (double)item / _wordCount).ToArray();

            return percents.Average();
        }

        private string ToFixedPercent(double value, int n = 2)
        {
            return (value * 100).ToString($"n{n}");
        }
    }
}