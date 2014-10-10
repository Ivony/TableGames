using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public abstract class GameHost : IGameHost
  {


    protected GameHost( Game game )
    {
      Game = game;
    }



    protected Game Game { get; private set; }



    /// <summary>
    /// 游戏状态
    /// </summary>
    public GameState GameState
    {
      get { return Game.GameState; }
    }

    public virtual bool TryJoinGame( IPlayerHost playerHost, out string reason )
    {
      var player = playerHost.GetPlayer();
      reason = null;

      if ( player != null )
      {
        if ( player.GameHost == this )
          return true;

        else
        {
          reason = "玩家已经加入其他游戏";
          return false;
        }
      }
      else
      {

        if ( Game.GameState != TableGame.GameState.NotStarted )
        {
          reason = "游戏已经开始或结束";
          return false;
        }

        player = Game.TryJoinGame( this, playerHost );
        if ( player == null )
        {
          reason = "未知原因";
          return false;
        }
        else
        {
          playerHost.JoinedGame( player );
          return true;
        }

      }


    }

    public Task Run()
    {
      lock ( SyncRoot )
      {
        if ( Game.GameState == GameState.NotInitialized )
          Game.Initialize();
      }


      return Game.Run();
    }





    public object SyncRoot
    {
      get { return Game.SyncRoot; }
    }

  }
}
