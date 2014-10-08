using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class WebGameHost<TGame> : GameHost<TGame> where TGame : Game
  {


    public WebGameHost( TGame game ) : base( game ) { }

    protected override async Task Run( GameProgress progress )
    {
      while ( true )
      {
        if ( !await progress.NextStep() )
          break;
      }
    }
  }
}