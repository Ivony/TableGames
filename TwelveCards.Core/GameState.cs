using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards
{
  public class GameState
  {

    private GameState( string state )
    {

    }


    public static readonly GameState NotInstallized = new GameState( "NotInstallized" );
    public static readonly GameState NotStarted = new GameState( "NotStarted" );
    public static readonly GameState InProgress = new GameState( "InProgress" );
    public static readonly GameState Finished = new GameState( "Finished" );

  }
}
