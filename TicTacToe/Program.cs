// 1. model ++
// 2. view +-
// 3. logic +-
// 4. ui



namespace TicTacToe
{
    internal class Program
    {

        class Player
        {
            public string Name { get; set; }
            public int Sign { get; set; }

            public Player(string name, int sign)
            {
                Name = name;
                Sign = sign;
            }
        }
        struct Coords
        {
            public int x;
            public int y;
        };
        enum GameState
        {
            Playing,
            Win,
            Tie,
            NotPlaying
        }


        static int size = 3;
        static int[,] field = new int[size, size];
        static GameState currentState = GameState.NotPlaying;

        static void Main(string[] args)
        {
            Menu();
        }

        static void _initField()
        {
            for (int i = 0; i < size; i++)    
                for (int j = 0; j < size; j++)
                    field[i, j] = 0;
        }
        static void _printHealine()
        {
            Console.WriteLine("     -------             -------                   -------        |--- ");
            Console.WriteLine("        |     o  |  /       |       /\\     |  /       |      --   |    ");
            Console.WriteLine("        |     |  | /        |      /  \\    | /        |     |  |  |--- ");
            Console.WriteLine("        |     |  | \\        |     /----\\   | \\        |     |  |  |    ");
            Console.WriteLine("        |     |  |  \\       |    /      \\  |  \\       |      --   |--- ");
        }
        static void _render()
        {
            Console.Write("\n\n\n");
            for (int i = 0; i < size; i++)
            {
                Console.Write("                                    ");
                for (int j = 0; j < size; j++)
                {
                    switch (field[i, j])
                    {
                        case 0:
                            Console.Write(' ');
                            break;
                        case 1:
                            Console.Write('o');
                            break;
                        case 2:
                            Console.Write('x');
                            break;
                    }
                    if ( j != size - 1)
                        Console.Write('|');

                }
                    
                Console.Write("\n                                    ");
                for (int k = 0; k < size + 2 && i != size - 1; k++)
                    Console.Write('-');
                Console.Write("\n");
            }
            Console.Write("\n\n\n");
        }

        static Coords GetCoords()
        {
            Coords coords = new Coords();

            string input = Console.ReadLine();
            while (input == "") input = Console.ReadLine();

            string[] temp = input.Trim().Split(',');
            coords.x = int.Parse(temp[0]);
            coords.y = int.Parse(temp[1]);

            return coords;
        }

       
        static void Menu()
        {
            Player[] players = new Player[2];


            Console.Clear();
            _printHealine();
            Console.WriteLine("\n\n---->Press 1 to play with friend...");

            ConsoleKey option = Console.ReadKey().Key;
            switch (option)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    for (int i = 0; i < 2; i++)
                    {
                        Console.WriteLine($"Enter player {i+1} name....");
                        string playerName = Console.ReadLine();
                        while (playerName == "")
                            playerName = Console.ReadLine();
                        players[i] = new(playerName, i + 1);
                    }
                    StartGame(players[0], players[1]);
                    break;
            }
        }

        static bool isNextStepAvailable()
        {
            for (int i = 0; i < size; i++) 
                for (int p = 1; p <= 2; p++)
                    if (
                        ((field[i, 0] == p || field[i, 0] == 0) && (field[i, 1] == p || field[i, 1] == 0) && (field[i, 2] == p || field[i, 2] == 0)) ||
                        ((field[0, i] == p || field[0, i] == 0) && (field[1, i] == p || field[1, i] == 0) && (field[2, i] == p || field[2, i] == 0))
                       )
                        return true;
            return false;
        }
        static bool isGameWinner()
        {

            for (int i = 0; i < size; i++)
            {
                //check for vertical and horizontal strike
                if ((field[i, 0] != 0 && field[i, 0] == field[i,  1] && field[i, 0] == field[i, 2]) ||
                    (field[0, i] != 0 && field[0, i] == field[1, i] && field[0, i] == field[2, i]))
                {
                    return true;
                }
            }
            //check for diagonal strike
            if (((field[0, 0] == field[1, 1] && field[0, 0] == field[2,2]) ||
                (field[2, 0] == field[1, 1] && field[2, 0] == field[0, 2])) &&
                 field[1, 1] != 0)
            {
                return true;
            }

            return false;
        }
        static void StartGame( Player player1, Player player2 )
        {
            Player lastMoveBy = player2;
            currentState = GameState.Playing;

            _initField();
            do
            {
                Console.Clear();
                _printHealine();
                _render();

                lastMoveBy = lastMoveBy.Name == player2.Name ? player1 : player2;
                Console.WriteLine($"Now is {lastMoveBy.Name} turn: ");
                Coords vector = GetCoords();


                // is coord out of field
                while (vector.x <= 0 || vector.y <= 0 || vector.x > 3 || vector.y > 3)
                {
                    Console.WriteLine("Enter correct coordinates!!");
                    vector = GetCoords();
                }
                // is cell empty + 
                while (field[vector.x - 1, vector.y - 1] != 0)
                {
                    Console.WriteLine("This cell is marked!!");
                    vector = GetCoords();
                }


                field[vector.x - 1, vector.y - 1] = lastMoveBy.Sign;


                if (!isNextStepAvailable())
                {
                    currentState = GameState.Tie;
                }
                if (isGameWinner())
                {
                    currentState = GameState.Win;
                } 

            }
            while (currentState == GameState.Playing);

            Console.Clear();
            _printHealine();
            _render();

            if (currentState == GameState.Win)
                Console.WriteLine($"Winner: {lastMoveBy.Name}. Congraduations!!");
            else if (currentState == GameState.Tie)
                Console.WriteLine("Tie. Congraduations!!");

            Console.WriteLine("\n\n---->Press -R- to back to main menu");

            ConsoleKey wait = Console.ReadKey().Key;
            while (wait!= ConsoleKey.R)
                wait = Console.ReadKey().Key;
            Menu();
            
        }

        
    }
}