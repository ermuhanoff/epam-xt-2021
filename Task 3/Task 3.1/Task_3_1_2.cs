using System;
using System.Linq;
using System.Collections.Generic;

namespace Task_3_1
{
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