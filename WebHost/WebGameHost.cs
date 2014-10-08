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



    public void JoinGame( IPlayerHost player )
    {
      string reason;
      if ( !TryJoinGame( player, out reason ) )
        player.Console.WriteMessage( new SystemMessage( string.Format( "加入游戏 \"{0}\" 失败，原因为： {1}", Game.Name, reason ) ) );

    }



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