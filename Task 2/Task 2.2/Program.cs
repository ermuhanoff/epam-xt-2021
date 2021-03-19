using System;
using System.Collections.Generic;

namespace Task_2_2
{
    class Program
    {
        static void Main(string[] args)
        {
            App app = new App();
            app.Start();

            // Console.ReadKey();
        }
    }

    public class Point
    {
        private int x;
        private int y;

        public int X { get => x; }
        public int Y { get => y; }

        public Point(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public override string ToString()
        {
            return $"[{x}:{y}]";
        }
    }
    public abstract class Figure
    {
        private Point pos;

        public Point Pos { get => pos; }

        public Figure(int x, int y)
        {
            pos = new Point(x, y);
        }

        public virtual void Display()
        {
            Console.WriteLine($"{this.GetType().Name} at {pos}");
        }
    }
    public abstract class FigureArea : Figure
    {
        public FigureArea(int x, int y) : base(x, y) { }

        public virtual double Area() { return 0; }

        public override void Display()
        {
            base.Display();
            Console.WriteLine($"Area: {Area()}");
        }
    }
    public class Line : Figure
    {
        private Point posEnd;
        public Point PosEnd { get => posEnd; }
        public Line(int sX, int sY, int eX, int eY) : base(sX, sY)
        {
            posEnd = new Point(eX, eY);
        }

        public double Length()
        {
            return Math.Sqrt(Math.Pow(posEnd.X - Pos.X, 2) + Math.Pow(posEnd.Y - Pos.Y, 2));
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine($"Length: {Length()}");
        }
    }
    public class Circum : Figure
    {
        private double radius;

        public double Radius { get => radius; }

        public Circum(int x, int y, double _radius) : base(x, y)
        {
            radius = _radius;
        }

        public double Length()
        {
            return 2 * Math.PI * radius;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine($"Radius: {radius}");
            Console.WriteLine($"Length: {Length()}");
        }
    }
    public class Circle : FigureArea
    {
        double radius;
        public double Radius { get => radius; }

        public Circle(int x, int y, double _radius) : base(x, y)
        {
            radius = _radius;
        }

        public double Length()
        {
            return 2 * Math.PI * radius;
        }

        public override double Area()
        {
            return Math.PI * radius * radius;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine($"Radius: {radius}");
            Console.WriteLine($"Length: {Length()}");
        }
    }
    public class Rectangle : FigureArea
    {
        private int width;
        private int height;

        public int Width { get => width; }
        public int Height { get => height; }
        public Rectangle(int x, int y, int _width, int _height) : base(x, y)
        {
            width = _width;
            height = _height;
        }

        public override double Area()
        {
            return width * height;
        }

        public int Perimeter()
        {
            return width * 2 + height * 2;
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine($"Perimeter: {Perimeter()}");
        }
    }
    public class Square : FigureArea
    {
        private int side;

        public int Side { get => side; }

        public Square(int x, int y, int _side) : base(x, y)
        {
            side = _side;
        }

        public override double Area()
        {
            return side * side;
        }
    }
    public class Trinangle : FigureArea
    {
        private Line baseLine;
        private Line sideLine1;
        private Line sideLine2;

        public Line BaseLine { get => baseLine; }
        public Trinangle(int hX, int hY, int lX1, int lY1, int lX2, int lY2) : base(hX, hY)
        {
            baseLine = new Line(lX1, lY1, lX2, lY2);
            sideLine1 = new Line(hX, hY, lX2, lY2);
            sideLine2 = new Line(hX, hX, lX1, lY1);
        }

        public double Perimeter()
        {
            return (baseLine.Length() + sideLine1.Length() + sideLine2.Length());
        }

        public override double Area()
        {
            double halfPerimetr = Perimeter() / 2;
            return Math.Sqrt(
            halfPerimetr *
            (halfPerimetr - baseLine.Length()) * (halfPerimetr - sideLine1.Length()) * (halfPerimetr - sideLine2.Length())
            );
        }

        public override void Display()
        {
            base.Display();
            Console.WriteLine($"Perimeter: {Perimeter()}");
        }
    }
    public class Ring : FigureArea
    {
        private int radiusIn;
        private int radiusOut;

        public int RadiusIn { get => radiusIn; }
        public int RadiusOut { get => radiusOut; }
        public Ring(int x, int y, int _radiusIn, int _radiusOut) : base(x, y)
        {
            radiusIn = _radiusIn;
            radiusOut = _radiusOut;
        }

        public double InnerLength()
        {
            return 2 * Math.PI * radiusIn;
        }

        public double OuterLength()
        {
            return 2 * Math.PI * radiusOut;
        }

        public double SumLength()
        {
            return OuterLength() + InnerLength();
        }

        public override double Area()
        {
            return Math.PI * (Math.Pow(radiusIn, 2) - Math.Pow(radiusOut, 2));
        }

    }

    public class App
    {
        private MenuController menuController;
        private FigureController figureController;
        private UserController userController;

        private User currentUser;

        public MenuController MenuController { get => menuController; }
        public FigureController FigureController { get => figureController; }
        public UserController UserController { get => userController; }
        public User CurrentUser { get => currentUser; }

        public App()
        {
            menuController = new MenuController();
            figureController = new FigureController();
            userController = new UserController();
            Init();
        }

        public void Init()
        {
            menuController.AddMenu("start",
               new string[] { "Choose user", "Add new user" },
               new Action<int>[] {
                    (_) =>
                    {
                        currentUser = userController.ChangeUser();
                        if (currentUser != null) {
                            InputController.LogMessage($"Current user is '{currentUser.Id}'");
                            figureController.LoadFigures(currentUser.Figures);
                            menuController.OpenMenu("main");
                        }
                    },
                    (_) =>
                    {
                        string username = InputController.InputString("username");
                        userController.TryAddUser(username);
                    },
               },
               false, true);
            menuController.AddMenu("main",
                new string[] { "Add figure", "Display all figures", "Clear canvas", "Change user" },
                new Action<int>[] {
                    (_) => menuController.OpenMenu("figures"),
                    (_) => figureController.DisplayAll(),
                    (_) =>
                    {
                        figureController.Clear();
                        currentUser.ClearFigureList();
                        InputController.LogMessage("Canvas cleared");
                    },
                    (_) => menuController.OpenMenu("start"),
                },
                false, true);
            menuController.AddMenuFromData("figures",
               figureController.GetFigureTypes(),
               (figureType) =>
               {
                   Figure figure = figureController.Add(figureType);
                   currentUser.AddFigure(figure);
               },
               true, false);
        }
        public void Start()
        {
            menuController.OpenMenu("start");
        }
    }

    public class Menu
    {
        private string[] options;
        private Action<int>[] actions;
        private bool canBack;
        private bool canExit;
        private string id;

        public string Id { get => id; }
        public string[] Options { get => options; }
        public Action<int>[] Actions { get => actions; }
        public bool CanBack { get => canBack; }
        public bool CanExit { get => canExit; }

        public Menu(string _id, string[] optionsList, Action<int>[] actionsList, bool back, bool exit)
        {
            id = _id;
            options = optionsList;
            actions = actionsList;
            canBack = back;
            canExit = exit;
        }

        public void Execute(int actionId, int dataId = 0)
        {
            actions[actionId].Invoke(dataId);
        }

        public void Display()
        {
            InputController.LogMessage("|  Select an action:  |");
            InputController.LogSep();
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            if (canBack)
            {
                Console.WriteLine($"{options.Length + 1}. Back");
            }
            if (canExit)
            {
                Console.WriteLine("0. Exit");
            }
        }
    }

    public class MenuController
    {
        private string activeMenu = String.Empty;
        private string prevMenu = String.Empty;
        private List<Menu> menuList = new List<Menu>();

        public string ActiveMenu { get => activeMenu; }
        public string PrevMenu { get => prevMenu; }
        public List<Menu> MenuList { get => menuList; }

        public void AddMenu(string id, string[] options, Action<int>[] actions, bool displayBack = false, bool displayExit = false)
        {
            menuList.Add(new Menu(id, options, actions, displayBack, displayExit));
        }
        public void AddMenuFromData<T>(string id, IEnumerable<T> data, Action<int> action, bool displayBack = false, bool displayExit = false)
        {
            List<string> options = new List<string>();
            foreach (T item in data)
            {
                options.Add(item.ToString());
            }

            menuList.Add(new Menu(id, options.ToArray(), new Action<int>[] { action }, displayBack, displayExit));
        }
        public void OpenMenu(string id)
        {
            Menu menu = GetMenuFromId(id);
            prevMenu = activeMenu;
            activeMenu = menu.Id;
            do
            {
                menu.Display();

                InputController.LogSep();

                bool err = Int32.TryParse(Console.ReadLine(), out int actionId);

                if (actionId < 0 || actionId > menu.Options.Length + 1 || !err)
                {
                    InputController.LogMessage($"Action '{actionId}' does not exist.");
                    continue;
                }

                if (actionId == 0)
                {
                    break;
                }
                if (actionId == menu.Options.Length + 1)
                {
                    OpenMenu(prevMenu);
                }
                if (menu.Actions.Length == 1)
                {
                    menu.Execute(0, actionId - 1);
                }
                else
                {
                    menu.Execute(actionId - 1);
                }

            }
            while (true);
        }
        public Menu GetMenuFromId(string id)
        {
            foreach (Menu menu in menuList)
            {
                if (menu.Id == id) return menu;
            }
            return null;
        }
    }

    public static class InputController
    {
        public static string InputString(string msg)
        {
            LogMessage($"Input {msg}:");
            string s = Console.ReadLine();
            return s;
        }
        public static double InputDouble(string msg)
        {
            LogMessage($"Input {msg}:");
            double res;
            bool exit;
            do
            {
                exit = Double.TryParse(Console.ReadLine(), out res);

                if (!exit || res <= 0)
                {
                    exit = false;
                    LogMessage("Wrong value! Try again.");
                }
            } while (!exit);

            return res;
        }
        public static int InputInt(string msg)
        {
            int res;
            bool exit;
            do
            {
                LogMessage($"Input {msg}:");
                exit = Int32.TryParse(Console.ReadLine(), out res);

                if (!exit || res <= 0)
                {
                    exit = false;
                    LogMessage("Wrong value! Try again.");
                }
            } while (!exit);

            return res;
        }

        public static (int x, int y) InputPoint(string msg)
        {
            LogMessage($"Input a coords of {msg}:");
            int x = InputController.InputInt("x coord");
            int y = InputController.InputInt("y coord");
            return (x, y);
        }
        public static void LogMessage(string msg)
        {
            LogSep();
            Console.WriteLine(msg);
        }
        public static void LogSep()
        {
            Console.WriteLine("-------------------");
        }
    }

    public class FigureController
    {
        private List<Figure> figureList;
        private enum figureTypes
        {
            TRIANGLE = 0,
            RECTANGLE,
            CIRCLE,
            CIRCUM,
            LINE,
            SQUARE,
            RING
        }

        public List<Figure> FigureList { get => figureList; }
        public FigureController()
        {
            figureList = new List<Figure>();
        }

        public void DisplayAll()
        {
            if (figureList.Count == 0)
            {
                InputController.LogMessage("No figures");
            }
            else
            {
                InputController.LogMessage("--");
                foreach (Figure f in figureList)
                {
                    f.Display();
                    Console.WriteLine("--");
                }

            }
            InputController.LogMessage("Press any key to back");
            Console.ReadKey();
            Console.WriteLine();
        }

        public void Clear()
        {
            figureList.Clear();
        }

        public void LoadFigures(List<Figure> figures)
        {
            figureList = new List<Figure>(figures);
            InputController.LogMessage("Figures loaded from current user!");
        }

        public string[] GetFigureTypes()
        {
            List<string> types = new List<string>();

            foreach (var type in Enum.GetValues(typeof(figureTypes)))
            {
                types.Add(type.ToString());
            }

            return types.ToArray();
        }

        public Figure Add(int figureType)
        {
            Figure figure = null;
            switch ((figureTypes)figureType)
            {
                case figureTypes.TRIANGLE:
                    var (tStartX1, tStartY1) = InputController.InputPoint("start point of height");
                    var (tStartX2, tStartY2) = InputController.InputPoint("start point of base");
                    var (tEndX1, tEndY2) = InputController.InputPoint("end point of base");
                    figure = new Trinangle(tStartX1, tStartY1, tStartX2, tStartY2, tEndX1, tEndY2);
                    break;
                case figureTypes.RECTANGLE:
                    var (rX, rY) = InputController.InputPoint("start point of Rectangle");
                    int w = InputController.InputInt("width");
                    int h = InputController.InputInt("height");
                    figure = new Rectangle(rX, rY, w, h);
                    break;
                case figureTypes.CIRCLE:
                    var (clX, clY) = InputController.InputPoint("center of Circle");
                    int radiusCl = InputController.InputInt("radius");
                    figure = new Circle(clX, clY, radiusCl);
                    break;
                case figureTypes.CIRCUM:
                    var (cmX, cmY) = InputController.InputPoint("center of Circum");
                    int radiusCm = InputController.InputInt("radius");
                    figure = new Circum(cmX, cmY, radiusCm);
                    break;
                case figureTypes.LINE:
                    var (lStartX, lStartY) = InputController.InputPoint("start point of Line");
                    var (lEndX, lEndY) = InputController.InputPoint("end point of Line");
                    figure = new Line(lStartX, lStartY, lEndX, lEndY);
                    break;
                case figureTypes.SQUARE:
                    var (sX, sY) = InputController.InputPoint("start point of Square");
                    int side = InputController.InputInt("side");
                    figure = new Square(sX, sY, side);
                    break;
                case figureTypes.RING:
                    var (rgX, rgY) = InputController.InputPoint("center of Ring");
                    bool exit = false;
                    int raduusIn;
                    int raduusOut;
                    do
                    {
                        exit = true;
                        raduusIn = InputController.InputInt("inner radius");
                        raduusOut = InputController.InputInt("outer radius");
                        if (raduusIn >= raduusOut)
                        {
                            InputController.LogMessage("Outer radius must be greater than inner radius!");
                            exit = false;
                        }
                    }
                    while (!exit);
                    figure = new Ring(rgX, rgY, raduusIn, raduusOut);
                    break;
            }

            figureList.Add(figure);
            return figure;
        }
    }

    public class User
    {
        private string id;
        private List<Figure> figures;

        public string Id { get => id; }
        public List<Figure> Figures { get => figures; }

        public User(string _id)
        {
            id = _id;
            figures = new List<Figure>();
        }

        public void AddFigure(Figure figure)
        {
            figures.Add(figure);
        }
        public void ClearFigureList()
        {
            figures.Clear();
        }

    }

    public class UserController
    {
        private List<User> userList;

        public List<User> UserList { get => userList; }
        public UserController()
        {
            userList = new List<User>();
            AddUser("admin"); //
        }

        public User ChangeUser()
        {
            if (userList.Count == 0)
            {
                InputController.LogMessage("No users.");
                return null;
            }
            string username = InputController.InputString("username");
            User user = FindUser(username);

            if (user == null)
            {
                InputController.LogMessage($"User '{username}' not found.");
                return null;
            }

            return user;
        }

        public int AddUser(string username)
        {
            if (FindUser(username) != null)
            {
                return -1;
            }
            else
            {
                userList.Add(new User(username));
                return 1;
            }
        }

        public void TryAddUser(string username)
        {
            int code = AddUser(username);
            if (code == -1)
            {
                InputController.LogMessage($"User '{username}' already exist.");
            }
            else if (code == 1)
            {
                InputController.LogMessage($"User '{username}' added.");
            }
        }

        public User FindUser(string id)
        {
            foreach (User u in userList)
            {
                if (u.Id == id) return u;
            }
            return null;
        }
    }
}
