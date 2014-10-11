using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class WebGameHost : GameHost
  {


    public WebGameHost( Game game ) : base( game ) { }



    public void JoinGame( IPlayerHost player )
    {
      string reason;
      if ( !TryJoinGame( player, out reason ) )
        player.WriteWarningMessage( "加入游戏 \"{0}\" 失败，原因为： {1}", Game.Name, reason );

    }


    public override Task Run()
    {
      return Task.Run( () => base.Run() );
    }

  }
}