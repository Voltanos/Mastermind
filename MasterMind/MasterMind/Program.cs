using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExceptionTracker.SOLID.ExceptionTracker.Model;
using MasterMindLibrary.SOLID.Game.Model;

namespace MasterMind
{
    class Program
    {
        static void Main(string[] args)
        {
            ///////////////////////////////////////////////
            //Local Variables

            string path = Environment.CurrentDirectory;
            string fileName = "ExceptionLog.txt";

            int inputLength = 4;
            int maxGuesses = 10;

            List<int> validNumbers = new List<int> { 1, 2, 3, 4, 5, 6 };

            Game game;

            TextFileLogger logger;

            ///////////////////////////////////////////////

            logger = new TextFileLogger(path, fileName);

            game = new Game(logger);

            game.InitializeGame(inputLength, maxGuesses, validNumbers);
            game.StartGameLoop();

            Console.Write("\n\nPress any key to close the console.  Thank you for playing!  :)");
            Console.ReadKey(true);
        }
    }
}
