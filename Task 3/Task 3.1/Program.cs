using System;

namespace Task_3_1
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
}