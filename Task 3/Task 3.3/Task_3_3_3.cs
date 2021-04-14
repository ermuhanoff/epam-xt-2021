using System;
using System.Linq;
using System.Collections.Generic;

namespace Task_3_3
{
    public class Pizzeria
    {
        private OrderOutput _orderOutput;
        private OrderWindow _orderWindow;
        private OrderBoard _orderBoard;
        private Kitchen _kitchen;
        public string Name { get; private set; }

        public Pizzeria(string name)
        {
            Name = name;

            _orderOutput = new OrderOutput();
            _kitchen = new Kitchen(_orderOutput);
            _orderWindow = new OrderWindow(_kitchen);
            _orderBoard = new OrderBoard(_kitchen, _orderOutput);
        }

        public int MakeOrder(params PizzaDescription[] descriptions)
        {
            return _orderWindow.CreateOrder(descriptions);
        }

        public void ShowInformation()
        {
            Console.WriteLine(_orderBoard.GetInformation());
        }

        public Pizza[] GetPizza(int number)
        {
            return _orderOutput.GetPizza(number);
        }
    }

    class OrderWindow
    {
        private Kitchen _kitchen;

        public OrderWindow(Kitchen kitchen)
        {
            _kitchen = kitchen;
        }

        public int CreateOrder(params PizzaDescription[] descriptions)
        {
            Order newOrder = new Order(descriptions);

            _kitchen.AddOrderToQueue(newOrder);

            return newOrder.Number;
        }
    }
    class OrderBoard
    {
        private Kitchen _kitchen;
        private OrderOutput _orderOutput;

        public OrderBoard(Kitchen kitchen, OrderOutput orderOutput)
        {
            _kitchen = kitchen;
            _orderOutput = orderOutput;
        }

        public string GetInformation()
        {
            return String.Join<OrderInfo>("\n", GetOrdersInfo());
        }

        public OrderInfo[] GetOrdersInfo()
        {
            Order[] fromKitchen = _kitchen.GetAllOrders();
            Order[] fromOutput = _orderOutput.GetAllOrders();

            return CreateOrderInfoList(fromKitchen, fromOutput).ToArray();
        }

        private List<OrderInfo> CreateOrderInfoList(params Order[][] orderList)
        {
            List<OrderInfo> ordersInfo = new List<OrderInfo>();

            foreach (Order[] array in orderList)
            {
                foreach (Order order in array)
                {
                    ordersInfo.Add(new OrderInfo(order));
                }
            }

            return ordersInfo;
        }
    }
    class OrderOutput
    {
        private Dictionary<int, Pizza[]> _packedPizza;
        private List<Order> _orders;

        public OrderOutput()
        {
            _packedPizza = new Dictionary<int, Pizza[]>();
            _orders = new List<Order>();
        }

        public void AddPizza(Pizza[] pizza, Order order)
        {
            _packedPizza.Add(order.Number, pizza);

            _orders.Add(order);
            order.State = Order.OrderState.READY;
        }

        public Pizza[] GetPizza(int orderNumber)
        {
            if (_packedPizza.ContainsKey(orderNumber))
            {
                Pizza[] pizzaArr = _packedPizza[orderNumber];

                RemovePizza(orderNumber);

                return pizzaArr;
            }

            throw new Exception("There is no such order, or this order is not yet READY!");
        }

        public Order[] GetAllOrders()
        {
            return _orders.ToArray();
        }

        private void RemovePizza(int orderNumber)
        {
            _packedPizza.Remove(orderNumber);
            _orders.RemoveAll(order => order.Number == orderNumber);
        }
    }
    class Kitchen
    {
        private List<Order> _orders;

        private OrderOutput _orderOutput;
        private Order _currentOrder = null;

        public Kitchen(OrderOutput orderOutput)
        {
            _orderOutput = orderOutput;
            _orders = new List<Order>();
        }

        public void AddOrderToQueue(Order order)
        {
            _orders.Add(order);

            if (_currentOrder == null)
            {
                AddOrderToCook();
            }
        }

        public void AddOrderToCook()
        {
            _currentOrder = _orders[0];
            _orders.RemoveAt(0);

            Cook();
        }

        public Order[] GetAllOrders()
        {
            List<Order> tmp = new List<Order>(_orders);

            if (_currentOrder != null)
            {
                tmp.Add(_currentOrder);
            }

            return tmp.ToArray();
        }

        private void Cook()
        {
            _currentOrder.State = Order.OrderState.COOKING;

            // some cook logic...

            RemoveFromCook();
        }
        private void RemoveFromCook()
        {
            List<Pizza> pizzaList = new List<Pizza>();

            foreach (PizzaDescription desc in _currentOrder.GetOrderList())
            {
                pizzaList.Add(new Pizza(desc));
            }

            _orderOutput.AddPizza(pizzaList.ToArray(), _currentOrder);

            _currentOrder = null;

            if (_orders.Count != 0)
            {
                AddOrderToCook();
            }
        }
    }
    public class Order
    {
        private static Random _random = new Random();
        private List<PizzaDescription> _descriptions;

        public enum OrderState
        {
            WAITING,
            COOKING,
            PACKING,
            READY
        }
        public int Number { get; }
        public OrderState State { get; set; }

        public int CookTime { get; }

        public Order(params PizzaDescription[] descriptions)
        {
            _descriptions = new List<PizzaDescription>();

            foreach (PizzaDescription desc in descriptions)
            {
                _descriptions.Add(desc);
            }

            Number = _random.Next(1000, 9999);
            CookTime = _random.Next(1, 2);
            State = OrderState.WAITING;
        }

        public PizzaDescription[] GetOrderList()
        {
            return _descriptions.ToArray();
        }

        public override string ToString()
        {
            return $"#{Number} - {State.ToString()}";
        }
    }
    public class Pizza
    {
        public enum PizzaType
        {
            MARGARITA,
            SICILIAN,
            HAWAIIAN,
            PEPPERONI
        }
        public PizzaType Type { get; }
        public int Size { get; }

        public Pizza(PizzaDescription description)
        {
            Type = description.Type;
            Size = description.Size;
        }
    }
    public struct PizzaDescription
    {
        public Pizza.PizzaType Type { get; }
        public int Size { get; }

        public PizzaDescription(Pizza.PizzaType type, int size)
        {
            Type = type;
            Size = size;
        }

        public override string ToString()
        {
            return $"{Type}({Size}см)";
        }
    }

    public struct OrderInfo
    {
        public Order.OrderState State { get; }
        public int Number { get; }
        public PizzaDescription[] OrderList { get; }

        public OrderInfo(Order order)
        {
            State = order.State;
            Number = order.Number;
            OrderList = order.GetOrderList();
        }

        public override string ToString()
        {
            string pizzaList = String.Join<PizzaDescription>(", ", OrderList);

            return $"#{Number} - {pizzaList} - {State.ToString()}";
        }
    }
}

