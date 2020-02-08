using System;

namespace MineSweeper
{
    class Program
    {
        public int Width;
        public int Height;
        public int ChoiceX;
        public int ChoiceY;
        public int WidthMinusOne;
        public int HeightMinusOne;
        public Program()
        { }
        public enum Operation { SetDefaults, ReDraw, SetValue };
        static void Main(string[] args)
        {
            Console.Clear();
            Program Storage = new Program();
            int Choice;
            int Mine;
        Start:
            Console.WindowWidth = 80;
            Console.WindowHeight = 15;
            Console.Clear();
            DrawCenter("Welcome To Minesweeper!");
            Console.WriteLine();
            Console.WriteLine();
            DrawCenter("A DIMA PRODUCTION");
            System.Threading.Thread.Sleep(1500);
            Console.Clear();
            DrawCenter("Enter in the Width Of Your Minefield:");
            Console.WriteLine();
            Storage.Width = Convert.ToInt32(Console.ReadLine());
            Storage.WidthMinusOne = Storage.Width - 1;
            if (Storage.Width > 43)
            {
                Console.WriteLine("The Maximum Width Is 43!");
                System.Threading.Thread.Sleep(2000);
                goto Start;
            }
            Console.Clear();
            DrawCenter("Enter in the Height Of Your Minefield");
            Console.WriteLine();
            Storage.Height = Convert.ToInt32(Console.ReadLine());
            Storage.HeightMinusOne = Storage.Height - 1;
            if (Storage.Height > 43)
            {
                Console.WriteLine("The Maximum Height Is 43!");
                System.Threading.Thread.Sleep(2000);
                goto Start;
            }
            Console.Clear();
            DrawCenter("Enter the percent Chance Of A Mine   (A Normal Game Is 10 - 20)");
            int MineChance = 100 / Convert.ToInt32(Console.ReadLine()) + 1;
            Console.WriteLine();
            int DefaultWidth = 25;
            if (Storage.Width * 4 < DefaultWidth)
            {
                Console.WindowWidth = DefaultWidth;
            }
            else
            {
                if (Storage.Width < 9)
                {
                    Console.WindowWidth = (Storage.Width * 4 + 6);
                }
                else
                {
                    Console.WindowWidth = (Storage.Width * 5 - 4);
                }
            }
            Console.WindowHeight = Storage.Height + 7;

            string[,] VisualGrid = new string[Storage.Width, Storage.Height];
            for (int y = 0; y < Storage.Height; y++)//loop that sets all of the original values
            {
                for (int x = 0; x < Storage.Width; x++)
                {
                    VisualGrid[x, y] = "[ ]";
                }
            }

            int[,] Array = new int[Storage.Width, Storage.Width];
            Random Rand = new Random();

            for (int x = 0; x < Storage.Height; x++) //sets all spaces to zero
            {
                for (int y = 0; y < Storage.Width; y++)
                {
                    Array[x, y] = 0;
                }
            }
            for (int y = 0; y < Storage.Height; y++) //sets the mines and numbers around them to +1
            {
                for (int x = 0; x < Storage.Width; x++)
                {
                    Mine = Rand.Next(0, MineChance);
                    if (Mine <= 0)//Mine is here
                    {
                        Array[x, y] = 9; //The number 9 means it is a mine because no non mine can ever be surrounded by 9 mines
                        if (y != 0)//Checks top box
                        {
                            if (Array[x, y - 1] != 9)//I had to type in all of these !=9's twice cuz I CTL+Z too much... (also they make sure it does not increment a mine)
                            {
                                Array[x, y - 1]++;
                            }
                        }
                        if (y != Storage.HeightMinusOne)//Checks Bottom box
                        {
                            if (Array[x, y+1] != 9)
                            {
                                Array[x, y+1]++;
                            }
                            if (x != 0)//Checks Bottom Left box
                            {
                                if (Array[x - 1, y + 1] != 9)
                                {
                                    Array[x - 1, y + 1]++;
                                }
                            }
                            if (x != Storage.HeightMinusOne)//Checks Bottom Right box
                            {
                                if (Array[x + 1, y + 1] != 9)
                                {
                                    Array[x + 1, y + 1]++;
                                }
                            }
                        }

                        if (x != 0)//Checks Left box And Left Side
                        {
                            if (Array[x-1, y] != 9)
                            {
                                Array[x-1, y]++;
                            }
                            if (y != 0)//Checks Top Left box
                            {
                                if (Array[x-1, y-1] != 9)
                                {
                                    Array[x-1, y-1]++;
                                }
                            }
                        }

                        if (x != Storage.WidthMinusOne)//Checks Right Box and Right Side
                        {
                            if (Array[x+1, y] != 9)
                            {
                                Array[x+1, y]++;
                            }
                            if (y != 0)//Checks Top Right box
                            {
                                if (Array[x+1, y-1] != 9)
                                {
                                    Array[x+1, y-1]++;
                                }
                            }
                        }
                    }
                }
            }//WOW, I had the write function in the math loop so I thought it could not write behind *FacePalm* *CRINGE*
        GuessAgain:
            Draw2(VisualGrid, Storage.Width, Storage.Height);
            Console.WriteLine();
            Console.WriteLine("Enter In Either A 1 Or 2:");
            Console.WriteLine();
            Console.WriteLine("1. Reveal A Square");
            Console.WriteLine("2. Flag A Square");
            Choice = Convert.ToInt32(Console.ReadLine());
            Draw2(VisualGrid, Storage.Width, Storage.Height);
            switch (Choice)
            {
                case 1: //Reveal A Square
                    Console.WriteLine();
                    Console.WriteLine("Enter In X Coord");
                    Storage.ChoiceX = Convert.ToInt32(Console.ReadLine());
                    Draw2(VisualGrid, Storage.Width, Storage.Height);
                    Console.WriteLine();
                    Console.WriteLine("Enter In Y Coord");
                    Storage.ChoiceY = Convert.ToInt32(Console.ReadLine());
                    //Finish Choice Making
                    if (VisualGrid[Storage.ChoiceX, Storage.ChoiceY] == "[?]")
                    {
                        break;
                    }
                    else if (Array[Storage.ChoiceX, Storage.ChoiceY] == 9)//if it is a mine
                    {
                        EndGame(VisualGrid,Array,Storage.Width, Storage.Height, Storage.ChoiceX, Storage.ChoiceY);
                        goto Start;
                    }
                    else if (Array[Storage.ChoiceX, Storage.ChoiceY] == 11)//if it was already guessed
                    {
                        Draw2(VisualGrid, Storage.ChoiceX, Storage.ChoiceY);
                        Console.WriteLine();
                        Console.WriteLine("Square Is Already Guessed!");
                        System.Threading.Thread.Sleep(2000);
                        break;
                    }
                    else if (Array[Storage.ChoiceX, Storage.ChoiceY] == 0)//if it is not surrounded by mines / search for empty squares function
                    {//This passes through everything and will only update the visual grid, not ever changing the official array values
                        VisualGrid = Scan(Array, Storage.WidthMinusOne, Storage.HeightMinusOne, Storage.ChoiceX, Storage.ChoiceY, VisualGrid);
                        Draw2(VisualGrid, Storage.Width, Storage.Height);
                        break;
                    }
                    else //this is not a mine and not a zero (all numbered squares 1-8)
                    {
                        VisualGrid[Storage.ChoiceX, Storage.ChoiceY] = "["+ Array[Storage.ChoiceX, Storage.ChoiceY] +"]";
                    }
                    break;
                case 2: //Flag A Square
                    Console.WriteLine();
                    Console.WriteLine("Enter In X Coord");
                    Storage.ChoiceX = Convert.ToInt32(Console.ReadLine());
                    Draw2(VisualGrid, Storage.Width, Storage.Height);
                    Console.WriteLine();
                    Console.WriteLine("Enter In Y Coord");
                    Storage.ChoiceY = Convert.ToInt32(Console.ReadLine());
                    if (VisualGrid[Storage.ChoiceX, Storage.ChoiceY] == "[?]")
                    {
                        VisualGrid[Storage.ChoiceX, Storage.ChoiceY] = "[ ]";
                    }
                    else if (VisualGrid[Storage.ChoiceX, Storage.ChoiceY] == "[ ]")
                    {
                        VisualGrid[Storage.ChoiceX, Storage.ChoiceY] = "[?]";
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("You Cant Flag Something That Is Already Revealed!");
                        System.Threading.Thread.Sleep(2500);
                    }
                    break;
            }
            int EmptySpaces = 0;
            for (int y = 0; y < Storage.Height; y++)//Loop that checks if every mine is flagged
            {
                for (int x = 0; x < Storage.Width; x++)
                {
                    if (VisualGrid[x,y] == "[ ]" && Array[x,y] != 9)
                    {
                        EmptySpaces++;
                    }
                }
            }
            if (EmptySpaces == 0)
            {
                WinGame(VisualGrid, Array, Storage.Width, Storage.Height);
                goto Start;
            }
            goto GuessAgain;
        }
        public static void DrawCenter(string TextToEnter)
        {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (TextToEnter.Length / 2)) + "}", TextToEnter));
        }
        public static void Draw2(string[,] Superb, int _W, int _H)
        {
            string[,] VisualGrid = Superb;
            Console.Clear();
            Console.Write("     ");//adds the space between left edge and x axis nubmering
            for (int x = 0; x < _W ; x++) //draw the numberd x axis
            {
                Console.Write("|" + x + "| ");
            }
            Console.WriteLine();
            for (int y = 0; y < _H; y++)//loop that shows all of the values
            {
                Console.Write("|"+y+ "|"); //draw the numbered y axis
                if (y < 10)
                {
                    Console.Write(" ");
                }
                if (y < 100)
                {
                    Console.Write(" ");
                }
                for (int x = 0; x < _W; x++)
                {
                    if (VisualGrid[x, y] == "[11]")
                    {
                        Console.Write("[0] ");
                        if (x > 9)
                        {
                            Console.Write(" ");
                        }
                        if (x > 99)
                        {
                            Console.Write(" ");
                        }
                    }
                    else
                    {
                        Console.Write(VisualGrid[x, y] + " ");
                        if (x > 9)
                        {
                            Console.Write(" ");
                        }
                        if (x > 99)
                        {
                            Console.Write(" ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }
        public static void WinGame(string[,] Visual, int[,] Logic, int _W, int _H)
        {
            for (int y = 0; y < _H; y++)//Loop that checks if every mine is flagged
            {
                for (int x = 0; x < _W; x++)
                {
                    if (Logic[x,y] == 9)
                    {
                        Visual[x, y] = "[X]";
                    }
                    else
                    {
                        Visual[x, y] = "["+ Logic[x, y] + "]";
                    }
                    
                }
            }
            Draw2(Visual, _W, _H);
            Console.WriteLine();
            Console.WriteLine("YOU WIN!");
            Console.WriteLine();
            Console.WriteLine("Congrats for beating Minesweeper!");
            Console.WriteLine();
            Console.WriteLine("Press Space To Reload The Game");
            Console.ReadKey();
            Console.Clear();
        }
        public static void EndGame(string[,] Visual, int[,] Logic, int _X, int _Y, int ChoiceX, int ChoiceY)
        {
            for (int y = 0; y < _Y; y++)//Loop that checks if every mine is flagged
            {
                for (int x = 0; x < _X; x++)
                {
                    if (Logic[x, y] == 9)
                    {
                        Visual[x, y] = "[X]";
                    }
                }
            }
            Draw2(Visual, _X, _Y);
            Console.WriteLine(); //Ends the game
            Console.WriteLine("GAME OVER");
            Console.WriteLine();
            Console.WriteLine("Mine Was At: " + "(" + ChoiceX + "," + ChoiceY + ")");
            Console.WriteLine();
            Console.WriteLine("Press Space To Restart");
            Console.ReadKey();
            Console.Clear();
        }
        public static string[,] Scan(int[,] _Array, int _W, int _H, int SelectedX, int SelectedY, string[,] _VisualGrid)
        {
            int[,] LogicArray = _Array;
            string[,] VisualGrid = _VisualGrid;
            //check itself
            if (LogicArray[SelectedX, SelectedY] == 0)//if the square is == 0
            {
                LogicArray[SelectedX, SelectedY] = 11;//set LogicArray to 11
                VisualGrid[SelectedX, SelectedY] = "[0]";// set visualgrid to 0
            }//no need to call the scan function on itself!
            if (SelectedX != 0)//if left square can be checked
            {
                if (LogicArray[SelectedX - 1, SelectedY] > 0 && LogicArray[SelectedX - 1, SelectedY] < 9) //1-8 can do this
                {
                    VisualGrid[SelectedX - 1, SelectedY] = "[" + LogicArray[SelectedX - 1, SelectedY] + "]";//set Visualgird to Logic array + []
                }
                if (LogicArray[SelectedX - 1, SelectedY] == 0)//if top square is == 0
                {
                    LogicArray[SelectedX - 1, SelectedY] = 11;//set LogicArray to 11
                    VisualGrid[SelectedX - 1, SelectedY] = "[0]";// set visualgrid to 0
                    Scan(LogicArray, _W, _H, SelectedX - 1, SelectedY, _VisualGrid); //scan the square scanned
                }
            }
            if (SelectedX != _W)//if right square can be checked
            {
                if (LogicArray[SelectedX + 1, SelectedY] > 0 && LogicArray[SelectedX + 1, SelectedY] < 9) //1-8 can do this
                {
                    VisualGrid[SelectedX + 1, SelectedY] = "[" + LogicArray[SelectedX + 1, SelectedY] + "]";//set Visualgird to Logic array + []
                }
                if (LogicArray[SelectedX + 1, SelectedY] == 0)//if top square is == 0
                {
                    LogicArray[SelectedX + 1, SelectedY] = 11;//set LogicArray to 11
                    VisualGrid[SelectedX + 1, SelectedY] = "[0]";// set visualgrid to 0
                    Scan(LogicArray, _W, _H, SelectedX + 1, SelectedY, _VisualGrid); //scan the square scanned
                }
            }

            if (SelectedY != 0)//if top square can be checked
            {
                if (LogicArray[SelectedX, SelectedY - 1] > 0 && LogicArray[SelectedX, SelectedY - 1] < 9) //1-8 can do this
                {
                    VisualGrid[SelectedX, SelectedY - 1] = "[" + LogicArray[SelectedX, SelectedY - 1] + "]";//set Visualgird to Logic array + []
                }
                if (LogicArray[SelectedX, SelectedY - 1] == 0)//if top square is == 0
                {
                    LogicArray[SelectedX, SelectedY - 1] = 11;//set LogicArray to 11
                    VisualGrid[SelectedX, SelectedY - 1] = "[0]";// set visualgrid to 0
                    Scan(LogicArray, _W, _H, SelectedX, SelectedY - 1, _VisualGrid); //scan the square scanned
                }

                if (SelectedX != 0)//if top left square can be checked
                {
                    if (LogicArray[SelectedX - 1, SelectedY - 1] > 0 && LogicArray[SelectedX - 1, SelectedY - 1] < 9) //1-8 can do this
                    {
                        VisualGrid[SelectedX - 1, SelectedY - 1] = "[" + LogicArray[SelectedX - 1, SelectedY - 1] + "]";//set Visualgird to Logic array + []
                    }
                    if (LogicArray[SelectedX - 1, SelectedY - 1] == 0)//if top left square is == 0
                    {
                        LogicArray[SelectedX - 1, SelectedY - 1] = 11;//set LogicArray to 11
                        VisualGrid[SelectedX - 1, SelectedY - 1] = "[0]";// set visualgrid to 0
                        Scan(LogicArray, _W, _H, SelectedX - 1, SelectedY - 1, _VisualGrid); //scan the square scanned
                    }
                }
                
                if (SelectedX != _W) //if top right square can be checked
                {
                    if (LogicArray[SelectedX + 1, SelectedY - 1] > 0 && LogicArray[SelectedX + 1, SelectedY - 1] < 9) //1-8 can do this
                    {
                        VisualGrid[SelectedX + 1, SelectedY - 1] = "[" + LogicArray[SelectedX + 1, SelectedY - 1] + "]";//set Visualgird to Logic array + []
                    }
                    if (LogicArray[SelectedX + 1, SelectedY - 1] == 0)//if top right square is == 0
                    {
                        LogicArray[SelectedX + 1, SelectedY - 1] = 11;//set LogicArray to 11
                        VisualGrid[SelectedX + 1, SelectedY - 1] = "[0]";// set visualgrid to 0
                        Scan(LogicArray, _W, _H, SelectedX + 1, SelectedY - 1, _VisualGrid); //scan the square scanned
                    }
                }
            }
            if (SelectedY != _H)//if bottom square can be checked
            {
                if (LogicArray[SelectedX, SelectedY + 1] > 0 && LogicArray[SelectedX, SelectedY + 1] < 9) //1-8 can do this
                {
                    VisualGrid[SelectedX, SelectedY + 1] = "[" + LogicArray[SelectedX, SelectedY + 1] + "]";//set Visualgird to Logic array + []
                }
                if (LogicArray[SelectedX, SelectedY + 1] == 0)//if Bottom square is == 0
                {
                    LogicArray[SelectedX, SelectedY + 1] = 11;//set LogicArray to 11
                    VisualGrid[SelectedX, SelectedY + 1] = "[0]";// set visualgrid to 0
                    Scan(LogicArray, _W, _H, SelectedX, SelectedY + 1, _VisualGrid); //scan the square scanned
                }
                if (SelectedX != _W)//if Bottom Right Square can be checked
                {
                    if (LogicArray[SelectedX + 1, SelectedY + 1] > 0 && LogicArray[SelectedX + 1, SelectedY + 1] < 9) //1-8 can do this
                    {
                        VisualGrid[SelectedX + 1, SelectedY + 1] = "[" + LogicArray[SelectedX + 1, SelectedY + 1] + "]";//set Visualgird to Logic array + []
                    }
                    if (LogicArray[SelectedX + 1, SelectedY + 1] == 0)//if Bottom square is == 0
                    {
                        LogicArray[SelectedX + 1, SelectedY + 1] = 11;//set LogicArray to 11
                        VisualGrid[SelectedX + 1, SelectedY + 1] = "[0]";// set visualgrid to 0
                        Scan(LogicArray, _W, _H, SelectedX + 1, SelectedY + 1, _VisualGrid); //scan the square scanned
                    }
                }
                if (SelectedX != 0)//if Bottom Left Square can be checked
                {
                    if (LogicArray[SelectedX - 1, SelectedY + 1] > 0 && LogicArray[SelectedX - 1, SelectedY + 1] < 9) //1-8 can do this
                    {
                        VisualGrid[SelectedX - 1, SelectedY + 1] = "[" + LogicArray[SelectedX - 1, SelectedY + 1] + "]";//set Visualgird to Logic array + []
                    }
                    if (LogicArray[SelectedX - 1, SelectedY + 1] == 0)//if Bottom square is == 0
                    {
                        LogicArray[SelectedX - 1, SelectedY + 1] = 11;//set LogicArray to 11
                        VisualGrid[SelectedX - 1, SelectedY + 1] = "[0]";// set visualgrid to 0
                        Scan(LogicArray, _W, _H, SelectedX - 1, SelectedY + 1, _VisualGrid); //scan the square scanned
                    }
                }
            }
            return VisualGrid;
        }

    }
}
