using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public abstract class GameProgress
  {


    protected GameProgress( Game game )
    {
      Game = game;
    }


    public Game Game { get; private set; }

    public abstract Task<bool> NextStep();


  }
}
