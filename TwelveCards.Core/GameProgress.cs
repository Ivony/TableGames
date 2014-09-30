using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards
{
  public class GameProgress
  {


    internal GameProgress( Game game )
    {
      Game = game;
    }


    public Game Game { get; private set; }

    public void NextStep()
    {


    }

  }
}
