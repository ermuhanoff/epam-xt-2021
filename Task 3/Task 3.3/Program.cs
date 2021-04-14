using System;

namespace Task_3_3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task 3.1

            int[] arr = { 1, 2, 3, 0, 5, 0, 4, 0, 2, 0 };

            Console.WriteLine($"Sum: {arr.Sum()}");
            Console.WriteLine($"Average: {arr.Average()}");
            Console.WriteLine($"Frequent: {arr.Frequent()}");

            arr = arr.Map((item, index) => item * item);

            arr.ForEach((item, index) => Console.WriteLine($"Element {index + 1}: {item}"));

            // Task 3.2

            string russian = "   !Привет мир!";
            string english = "Hello   world!";
            string number = "2021";
            string mixed = " Hello from Россия из 2021!";

            Console.WriteLine(russian.GetLanguageType());
            Console.WriteLine(english.GetLanguageType());
            Console.WriteLine(number.GetLanguageType());
            Console.WriteLine(mixed.GetLanguageType());

            // Task 3.3

            Pizzeria pizzeriaDada = new Pizzeria("Dada");

            int order1 = pizzeriaDada.MakeOrder(new PizzaDescription(Pizza.PizzaType.MARGARITA, 35), new PizzaDescription(Pizza.PizzaType.SICILIAN, 40));
            int order2 = pizzeriaDada.MakeOrder(new PizzaDescription(Pizza.PizzaType.MARGARITA, 30));
            int order3 = pizzeriaDada.MakeOrder(new PizzaDescription(Pizza.PizzaType.HAWAIIAN, 40));
            int order4 = pizzeriaDada.MakeOrder(new PizzaDescription(Pizza.PizzaType.PEPPERONI, 25));

            pizzeriaDada.ShowInformation();

            pizzeriaDada.GetPizza(order1);
            pizzeriaDada.GetPizza(order3);
        }
    }


}