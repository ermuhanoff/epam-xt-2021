using System.Threading;
using System.Text;
using System.Collections.Generic;
using System;

namespace Task_2_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(70, 31);

            Scene scene1 = new Scene(game, "main");

            Level level = new Level(scene1, 50, 30);

            int playerWinScore = 5;

            scene1.AddLevel(level);

            scene1.AddPlayer(new Player(scene1, game.Width / 2, game.Height / 2, new char[] { 'X' }, playerWinScore, "player"));

            level.SpawnLevelObjects(playerWinScore, new char[] { '*' }, "bonus");
            level.SpawnEnemyObjects(15, new char[] { '@' }, "enemy");

            scene1.AddText($"Score: 0", "player_score", game.Width, 0, 1);
            scene1.AddText($"♥♥♥", "player_health", game.Width, 1, 1);

            Scene scene2 = new Scene(game, "game_over");

            scene2.AddText($"GAME OVER", "game_over", game.Width / 2 - 5, game.Height / 2 - 1, 0);

            scene2.AddText($"EXIT", "game_exit", game.Width / 2 - 3, game.Height / 2 + 3, 0);

            GameObject cursorScene2 = new GameObject(scene2, game.Width / 2 - 5, game.Height / 2 + 3, new char[] { '>' }, "cursor");
            cursorScene2.AddScript(self =>
            {
                ConsoleKey? key = InputController.PressedKey();
                if (key == ConsoleKey.Enter)
                {
                    game.IsRunning = false;
                }

            });
            scene2.AddObject(cursorScene2);

            Scene scene3 = new Scene(game, "start");

            scene3.AddText($"START GAME", "game_start", game.Width / 2 - 10, game.Height / 2 - 1, 0);
            scene3.AddText($"EXIT", "game_exit", game.Width / 2 - 10, game.Height / 2, 0);

            GameObject cursorScene3 = new GameObject(scene3, game.Width / 2 - 12, game.Height / 2 - 1, new char[] { '>' }, "cursor");
            cursorScene3.AddScript(self =>
            {
                ConsoleKey? key = InputController.PressedKey();
                if (key == ConsoleKey.DownArrow)
                {
                    if (self.Y < game.Height / 2) { self.Y++; }
                }
                if (key == ConsoleKey.UpArrow)
                {
                    if (self.Y > game.Height / 2 - 1) { self.Y--; }
                }
                if (key == ConsoleKey.Enter)
                {
                    if (self.Y == game.Height / 2 - 1) { game.SetScene("main"); }
                    else if (self.Y == game.Height / 2) { game.IsRunning = false; }
                }

            });
            scene3.AddObject(cursorScene3);

            Scene scene4 = new Scene(game, "win");

            scene4.AddText($"YOU WIN", "win", game.Width / 2 - 5, game.Height / 2 - 1, 0);

            scene4.AddText($"EXIT", "game_exit", game.Width / 2 - 3, game.Height / 2 + 3, 0);

            scene4.AddObject(cursorScene2);

            game.AddScene(scene1);
            game.AddScene(scene2);
            game.AddScene(scene3);
            game.AddScene(scene4);

            game.SetScene("start");

            game.IsRunning = true;

            game.Start();
        }
    }

    public class Game
    {
        private int width;
        private int height;
        private char[] output;
        private bool isRunning;
        private Scene currentScene;
        private List<Scene> scenes;

        public char[] Output { get => output; }
        public int Width { get => width; }
        public int Height { get => height; }
        public Scene CurrentScene { get => currentScene; }
        public char CHAR_ERR = '\0';
        public bool IsRunning { get => isRunning; set => isRunning = value; }

        public Random random;

        public Game(int _width, int _height)
        {
            width = _width;
            height = _height;
            output = new char[(width * height)];
            Console.SetWindowSize(width, height + 1);
            Console.SetBufferSize(width, height + 1);
            Console.CursorVisible = false;

            scenes = new List<Scene>();
            random = new Random();
        }

        public void SetScene(string name)
        {
            foreach (Scene scene in scenes)
            {
                if (scene.Name == name)
                {
                    currentScene = scene;
                }
            }
        }
        public void AddScene(Scene scene)
        {
            scenes.Add(scene);
        }
        public char GetChar(int x, int y)
        {
            int index = GetScreenIndex(x, y);

            if (index != -1)
            {
                return Output[index];
            }
            return CHAR_ERR;
        }
        public int GetScreenIndex(int x, int y)
        {
            int index = y * width + x;
            if (index >= 0 || index < Output.Length)
            {
                return index;
            }
            return -1;
        }
        public void AddToOutput(char[] chars, int index)
        {
            if (index >= 0 && index + chars.Length <= Output.Length)
            {
                if (chars.Length == 1)
                {
                    Output[index] = chars[0];
                }
                else
                {
                    chars.CopyTo(Output, index);
                }
            }

        }
        public void Start()
        {
            while (isRunning)
            {
                Clear();
                currentScene.Update();
                Render();
            }
        }

        private void Clear()
        {
            Array.Clear(Output, 0, Output.Length);
            Console.SetCursorPosition(0, 0);
        }
        private void Render()
        {
            Console.Write(Output);
        }
    }

    public class Scene
    {
        private string name;
        private Player player;
        private Game game;
        public Level level;
        private List<GameObject> gameObjects;
        private List<GameObject> willRemoveObjects;
        public Level Level { get => level; }
        public string Name { get => name; }
        public Player Player { get => player; }
        public Game Game { get => game; }

        public Scene(Game _game, string _name)
        {
            name = _name;
            game = _game;

            gameObjects = new List<GameObject>();
            willRemoveObjects = new List<GameObject>();

        }
        public void AddObject(GameObject obj)
        {
            if (obj is Overlay)
            {
                gameObjects.Add(obj);
            }
            else
            {
                gameObjects.Insert(0, obj);
            }
        }
        private void RemoveObjects()
        {
            gameObjects.RemoveAll((obj) => willRemoveObjects.Contains(obj));
            willRemoveObjects.Clear();
        }

        public void AddLevel(Level _level)
        {
            level = _level;
        }
        public void AddPlayer(Player _player)
        {
            player = _player;
            AddObject(player);
        }
        public void RemoveObject(GameObject obj)
        {
            willRemoveObjects.Add(obj);
        }
        public GameObject GetObjectAt(int x, int y)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.X == x && obj.Y == y && !(obj is Player))
                {
                    return obj;
                }
            }
            return null;
        }
        public GameObject GetObjectBy(string name)
        {
            foreach (GameObject obj in gameObjects)
            {
                if (obj.Name == name)
                {
                    return obj;
                }
            }
            return null;
        }
        public void AddText(string _text, string name, int x, int y, int align)
        {
            Text text = new Text(this, x, y, _text, align, name);
            AddObject(text);
        }
        private void UpdateObjects()
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.Update();

                int index = game.GetScreenIndex(obj.X, obj.Y);
                if (index >= 0 && obj.Visible)
                {
                    game.AddToOutput(obj.Display(), index);
                }
            }
        }
        private void LevelBuild()
        {
            if (Level != null)
            {
                char[] chars = Level.Output;
                if (Level.Width != game.Width || Level.Height != game.Height)
                {
                    for (int i = 0; i < chars.Length / Level.Width; i++)
                    {
                        char[] part = new char[Level.Width];
                        Array.Copy(chars, i * Level.Width, part, 0, Level.Width);
                        game.AddToOutput(part, i * game.Width);
                    }
                }
                else
                {
                    game.AddToOutput(chars, 0);
                }
            }
        }
        public void Update()
        {
            RemoveObjects();
            LevelBuild();
            UpdateObjects();
        }
    }

    public class Level
    {
        private int width;
        private int height;
        private char[] levelOutput;
        public char CHAR_WALL = '\u2588';
        public int Width { get => width; }
        public int Height { get => height; }
        public char[] Output { get => levelOutput; }
        public Game game;
        public Scene scene;

        public Level(Scene _scene, int _width, int _height)
        {
            scene = _scene;
            game = scene.Game;
            width = _width;
            height = _height;
            levelOutput = GetChars();
        }

        public bool IsWallAt(int x, int y)
        {
            char wall = game.GetChar(x, y);
            if (wall == CHAR_WALL)
            {
                return true;
            }
            return false;
        }

        public (int x, int y) GetRandomCoord()
        {
            int x, y;
            bool isPlayerCoord;
            bool isSomeObjCoord;
            do
            {
                x = game.random.Next(1, width - 1);
                y = game.random.Next(1, height - 1);

                isPlayerCoord = scene.Player.X == x && scene.Player.Y == x;
                isSomeObjCoord = scene.GetObjectAt(x, y) != null;
            }
            while (IsWallAt(x, y) && isSomeObjCoord && isPlayerCoord);
            return (x, y);
        }

        public void SpawnLevelObjects(int count, char[] output, string name)
        {
            for (int i = 0; i < count; i++)
            {
                var (x, y) = GetRandomCoord();

                scene.AddObject(new LevelObject(scene, x, y, output, name + (i + 1)));
            }
        }

        public void SpawnEnemyObjects(int count, char[] output, string name)
        {
            for (int i = 0; i < count; i++)
            {
                var (x, y) = GetRandomCoord();

                scene.AddObject(new Enemy(scene, x, y, output, name + (i + 1)));
            }
        }

        public char[] GetChars()
        {
            string levelTempate = "";
            levelTempate += "##################################################";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#######################################          #";
            levelTempate += "#                                     #          #";
            levelTempate += "#                                     #          #";
            levelTempate += "#                                     #          #";
            levelTempate += "##################    ######################     #";
            levelTempate += "#                         #                      #";
            levelTempate += "#                         #                      #";
            levelTempate += "#                         #                      #";
            levelTempate += "#                         #                      #";
            levelTempate += "#                         #                      #";
            levelTempate += "#                         #                      #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#########################                        #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "#                                                #";
            levelTempate += "##################################################";

            return Build(levelTempate);
        }

        private char[] Build(string map)
        {
            char[] chars = map.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '#') chars[i] = CHAR_WALL;
            }

            return chars;
        }
    }

    public class GameObject
    {
        private int x;
        private int y;
        private char[] output;
        private string name;
        private List<Action<GameObject>> scripts;
        private Scene scene;

        public bool Visible;
        public char[] Output { get => output; set => output = value; }
        public string Name { get => name; }
        public Scene Scene { get => scene; }
        public Game Game { get => scene.Game; }

        public int X
        {
            get => x;
            set => x = value;
        }
        public int Y
        {
            get => y;
            set => y = value;
        }

        public GameObject(Scene _scene, int _x, int _y, char[] _output, string _name)
        {
            name = _name;
            scene = _scene;
            x = _x;
            y = _y;
            output = _output;
            Visible = true;
            scripts = new List<Action<GameObject>>();
        }

        public void AddScript(Action<GameObject> script)
        {
            scripts.Add(script);
        }

        public char[] Display()
        {
            return output;
        }

        private void ExecuteScripts()
        {
            foreach (Action<GameObject> script in scripts)
            {
                script.Invoke(this);
            }
        }

        public virtual void Update()
        {
            ExecuteScripts();
        }
    }

    public class Movable : GameObject
    {
        private int direction; // 0 - up 1 - down 2 - left 3 - right

        public int Direction { get => direction; }
        public Movable(Scene scene, int x, int y, char[] output, string name) : base(scene, x, y, output, name)
        {
            direction = 0;
        }

        public void MoveLeft()
        {
            if (!Scene.level.IsWallAt(X - 1, Y))
            {
                X = X - 1;
                direction = 2;
            }

        }
        public void MoveRight()
        {
            if (!Scene.level.IsWallAt(X + 1, Y))
            {
                X = X + 1;
                direction = 3;
            }
        }
        public void MoveUp()
        {
            if (!Scene.level.IsWallAt(X, Y - 1))
            {
                Y = Y - 1;
                direction = 0;
            }
        }
        public void MoveDown()
        {
            if (!Scene.level.IsWallAt(X, Y + 1))
            {
                Y = Y + 1;
                direction = 1;
            }
        }

        public virtual void Controller() { }
        public override void Update()
        {
            Controller();
            base.Update();
        }
    }

    public class Player : Movable
    {
        public int score;
        public int health;
        public int maxScore;
        public Player(Scene scene, int x, int y, char[] renderers, int _maxScore, string name) : base(scene, x, y, renderers, name)
        {
            score = 0;
            health = 3;
            maxScore = _maxScore;
        }

        public override void Controller()
        {

            switch (InputController.PressedKey())
            {
                case ConsoleKey.DownArrow:
                    // SetRenderer(1);
                    MoveDown();
                    break;
                case ConsoleKey.UpArrow:
                    // SetRenderer(0);
                    MoveUp();
                    break;
                case ConsoleKey.RightArrow:
                    // SetRenderer(3);
                    MoveRight();
                    break;
                case ConsoleKey.LeftArrow:
                    // SetRenderer(2);
                    MoveLeft();
                    break;
            }
        }

        private void Bonus()
        {
            GameObject obj = Scene.GetObjectAt(X, Y);

            if (obj is LevelObject)
            {
                score++;
                ((Text)Scene.GetObjectBy("player_score")).SetText($"Score: {score}");
                Scene.RemoveObject(obj);
            }
        }
        private void IsGameOver()
        {
            if (health <= 0)
            {
                Game.SetScene("game_over");
            }
        }
        private void IsWin()
        {
            if (score == maxScore)
            {
                Game.SetScene("win");
            }
        }
        public override void Update()
        {
            IsGameOver();
            IsWin();
            Bonus();
            base.Update();
        }
    }

    public static class InputController
    {
        public static ConsoleKey? PressedKey()
        {
            if (Console.KeyAvailable)
            {
                return Console.ReadKey().Key;
            }

            return null;
        }
    }

    public class LevelObject : GameObject
    {
        public LevelObject(Scene scene, int x, int y, char[] output, string name) : base(scene, x, y, output, name) { }
    }

    public class Enemy : Movable
    {
        int timeToMove;
        int timeMoveExpire;
        int timeToAttack;
        int timeAttackExpire;
        public Enemy(Scene scene, int x, int y, char[] output, string name) : base(scene, x, y, output, name)
        {
            timeToMove = 400;
            timeToAttack = 2000;
            timeAttackExpire = timeToAttack;
        }

        public void Attack()
        {
            if (timeAttackExpire == timeToAttack)
            {
                Player player = Scene.Player;

                if (player.X == X && player.Y == Y)
                {
                    player.health--;
                    ((Text)Scene.GetObjectBy("player_health")).SetText(player.health > 0 ? new String('♥', player.health) : "");
                    timeAttackExpire = 0;
                }
            }
            else { timeAttackExpire++; }
        }

        public override void Controller()
        {
            base.Controller();

            if (timeMoveExpire == timeToMove)
            {
                timeMoveExpire = 0;
                int dir = Game.random.Next(0, 4);

                switch (dir)
                {
                    case 0:
                        MoveUp();
                        break;
                    case 1:
                        MoveDown();
                        break;
                    case 2:
                        MoveLeft();
                        break;
                    case 3:
                        MoveRight();
                        break;
                }
            }
            timeMoveExpire++;
        }

        public override void Update()
        {
            Attack();
            base.Update();
        }
    }

    public abstract class Overlay : GameObject
    {
        public Overlay(Scene scene, int x, int y, char[] output, string name) : base(scene, x, y, output, name) { }
    }

    public class Text : Overlay
    {
        private int align;
        private int startX;
        private int startY;
        public Text(Scene scene, int x, int y, string text, int _align, string name) : base(scene, x, y, text.ToCharArray(), name)
        {
            align = _align;
            startX = x;
            startY = y;
            X = startX - (Output.Length * align);
        }

        public void SetText(string text)
        {
            Output = text.ToCharArray();
            X = startX - (Output.Length * align);
        }
    }
}



