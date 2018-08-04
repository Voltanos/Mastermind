using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MasterMindLibrary.SOLID.Game.Interfaces;
using ExceptionTracker.SOLID.ExceptionTracker.Model;

namespace MasterMindLibrary.SOLID.Game.Model
{
    public class Game : IStartGameLoop, IInitializeGame
    {
        #region Member Variables

        private const string MINUS_PEG = "-";  //Right code, wrong spot.
        private const string PLUS_PEG = "+";  //Right code, right spot.
        private const string ZERO_PEG = "0";  //Neutral peg.

        private TextFileLogger Logger;

        private bool GameLoopOn;

        private int InputLength;
        private int CurrentGuesses;
        private int MaxGuesses;

        private List<int> ValidNumbers;

        private List<string> ValidNumbersAsString;
        private List<string> PlayerInput;
        private List<string> MainCode;
        private List<string> HintCode;

        #endregion

        #region Constructors

        public Game(TextFileLogger logger)
        {
            this.Logger = logger;

            this.GameLoopOn = false;

            this.InputLength = -1;
            this.CurrentGuesses = -1;
            this.MaxGuesses = -1;

            this.ValidNumbers = new List<int>();

            this.ValidNumbersAsString = new List<string>();
            this.PlayerInput = new List<string>();
            this.MainCode = new List<string>();
            this.HintCode = new List<string>();
        }

        #endregion

        #region Public Methods

        public void InitializeGame(int inputLength, int maxGuesses, List<int> validNumbers)
        {
            this.InputLength = inputLength;
            this.MaxGuesses = maxGuesses;

            this.ValidNumbers = validNumbers;

            foreach (int value in this.ValidNumbers)
            {
                this.ValidNumbersAsString.Add(value.ToString());
            }
        }

        public void StartGameLoop()
        {
            try
            {
                TurnOnMainGameLoop();
            }

            catch (Exception ex)
            {
                this.Logger.ExceptionLog(ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Private Methods

        #region "StartGameLoop" methods

        private void TurnOnMainGameLoop()
        {
            this.GameLoopOn = true;

            while (this.GameLoopOn == true)
            {
                StartGuessLoop();
            }
        }

        private void StartGuessLoop()
        {
            this.CurrentGuesses = 0;
            CreateMainCode();

            while (this.CurrentGuesses < this.MaxGuesses)
            {
                GetPlayerInput();
            }

            CheckForReplay();
        }

        private void CreateMainCode()
        {
            ///////////////////////////////////////////
            //Local Variables

            Random random = new Random();

            ///////////////////////////////////////////

            this.MainCode.Clear();

            for (int i = 0; i < this.InputLength; i += 1)
            {
                GetRandomItemFromValidNumbers(random);
            }

            Debug.WriteLine(String.Format("Main Code:  {0}", PrintOutListHorizontally(this.MainCode)));
            this.Logger.Log(String.Format("Main Code:  {0}", PrintOutListHorizontally(this.MainCode)));
        }

        private void GetRandomItemFromValidNumbers(Random random)
        {
            /////////////////////////////////////////////
            //Local Variables

            int index = -1;

            /////////////////////////////////////////////

            index = random.Next(this.ValidNumbers.Count);

            this.MainCode.Add(this.ValidNumbers[index].ToString());
        }

        private void GetPlayerInput()
        {
            /////////////////////////////////////////////////
            //Local Variables

            string input;

            /////////////////////////////////////////////////

            Console.WriteLine("\n\n");
            Console.WriteLine("\nTurn:  {0}", this.CurrentGuesses);
            Console.WriteLine("\nMax Turns:  {0}", this.MaxGuesses);
            Console.WriteLine("\n\nEnter a {0}-digit long code using the following values:  {1}", this.InputLength, PrintOutListHorizontally(this.ValidNumbers));
            Console.WriteLine("\n?");
            var value = Console.ReadLine();

            input = value.ToString();

            this.PlayerInput.Clear();
            this.HintCode.Clear();

            for (int i = 0; i < input.Length; i += 1)
            {
                this.PlayerInput.Add(input[i].ToString());
            }

            ValidatePlayerInput();
        }

        private void ValidatePlayerInput()
        {
            if (PlayerInputIsEqualLength() == false)
            {
                return;
            }

            if (PlayerInputIsWithinValidNumbers() == false)
            {
                return;
            }

            for (int i = 0; i < this.PlayerInput.Count; i += 1)
            {
                PlayerInputIsEqualToMainCode(i);
            }

            CheckHintCode();
        }

        private void CheckHintCode()
        {
            if (HintCodeIsAllPlus() == true)
            {
                WinMessage();
            }

            else
            {
                IncrementTurn();
            }
        }

        private bool HintCodeIsAllPlus()
        {
            if ((this.HintCode.IndexOf(MINUS_PEG) == -1) && (this.HintCode.IndexOf(ZERO_PEG) == -1))
            {
                return true;
            }

            return false;
        }

        private void WinMessage()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("\nYou figured out the right code!  :)");
            Console.WriteLine("\nTotal guesses made:  {0}", this.CurrentGuesses);

            this.CurrentGuesses = this.MaxGuesses + 1;
        }

        private void IncrementTurn()
        {
            Console.WriteLine("\n\nYou entered:  {0}", PrintOutListHorizontally(this.PlayerInput));
            Console.WriteLine("\n\nYour hint:  {0}", PrintOutListHorizontally(this.HintCode));

            this.CurrentGuesses += 1;

            if (this.CurrentGuesses >= this.MaxGuesses)
            {
                LoseMessage();
            }
        }

        private void LoseMessage()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("\nSorry!  You didn't guess the code in time.  :(");
            Console.WriteLine("\nMain Code:  {0}", PrintOutListHorizontally(this.MainCode));
        }

        private void PlayerInputIsEqualToMainCode(int index)
        {
            if (this.PlayerInput[index].Equals(this.MainCode[index]) == true)
            {
                this.HintCode.Add(PLUS_PEG);
            }

            else
            {
                PlayerInputIsSomewhereInMainCode(index);
            }
        }

        private void PlayerInputIsSomewhereInMainCode(int index)
        {
            if (this.MainCode.IndexOf(this.PlayerInput[index]) > 0)
            {
                this.HintCode.Add(MINUS_PEG);
            }

            else
            {
                this.HintCode.Add(ZERO_PEG);
            }
        }        

        private bool PlayerInputIsWithinValidNumbers()
        {
            ///////////////////////////////////////////////
            //Local Variables

            bool value = true;

            ///////////////////////////////////////////////

            foreach (string input in this.PlayerInput)
            {
                if (this.ValidNumbersAsString.IndexOf(input) == -1)
                {
                    value = false;
                    break;
                }
            }

            return value;
        }

        private bool PlayerInputIsEqualLength()
        {
            if (this.PlayerInput.Count == this.MainCode.Count)
            {
                return true;
            }

            return false;
        }

        private string PrintOutListHorizontally(List<int> list)
        {
            /////////////////////////////////////////////////
            //Local Variables

            bool nextValue = false;

            StringBuilder builder = new StringBuilder();

            /////////////////////////////////////////////////

            foreach (int num in list)
            {
                if (nextValue == true)
                {
                    builder.Append(", ");
                }

                builder.AppendFormat("{0}", num);
                nextValue = true;
            }

            return builder.ToString();
        }

        private string PrintOutListHorizontally(List<string> list)
        {
            /////////////////////////////////////////////////
            //Local Variables

            bool nextValue = false;

            StringBuilder builder = new StringBuilder();

            /////////////////////////////////////////////////

            foreach (string num in list)
            {
                if (nextValue == true)
                {
                    builder.Append(", ");
                }

                builder.AppendFormat("{0}", num);
                nextValue = true;
            }

            return builder.ToString();
        }

        private void CheckForReplay()
        {
            /////////////////////////////////////////////////
            //Local Variables

            const string YES = "Y";

            string input;

            /////////////////////////////////////////////////

            Console.WriteLine("\nWould you like to play again (Y/N)?  ");
            var value = Console.ReadLine();

            input = value.ToString().ToUpper();

            if (input.Equals(YES) == false)
            {
                this.GameLoopOn = false;
            }
        }

        #endregion

        #endregion
    }
}
