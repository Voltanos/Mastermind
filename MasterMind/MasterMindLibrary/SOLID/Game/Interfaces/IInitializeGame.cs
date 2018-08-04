using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindLibrary.SOLID.Game.Interfaces
{
    interface IInitializeGame
    {
        void InitializeGame(int inputLength, int maxGuesses, List<int> validNumbers);
    }
}
