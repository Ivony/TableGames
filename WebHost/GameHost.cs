using Ivony.TableGame.SimpleGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class GameHost : GameHostBase
  {


    public GameHost( string roomName )
      : base( roomName )
    {
      _game = new SimpleGame( this );
      _game.Initialize();
    }



    public void JoinGame( IPlayerHost player )
    {
      string reason;
      if ( !TryJoinGame( player, out reason ) )
        player.WriteWarningMessage( "加入游戏 \"{0}\" 失败，原因为： {1}", Game.RoomName, reason );

    }


    public override Task Run()
    {
      return Task.Run( () => base.Run() );
    }


    private SimpleGame _game;
    public override GameBase Game
    {
      get { return _game; }
    }


    public override void ReleaseGame( GameBase game )
    {
      base.ReleaseGame( game );

      Games.ReleaseGameHost( this );
    }
  }
}