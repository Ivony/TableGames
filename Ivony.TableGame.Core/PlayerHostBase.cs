using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public abstract class PlayerHostBase : IPlayerHost
  {




    protected PlayerHostBase( string playerName )
    {
      Name = playerName;
      SyncRoot = new object();
    }

    /// <summary>
    /// 获取玩家名称
    /// </summary>
    public string Name { get; protected set; }




    public object SyncRoot
    {
      get;
      private set;
    }



    private PlayerConsoleBase _console;
    public virtual PlayerConsoleBase Console
    {
      get
      {
        lock ( SyncRoot )
        {
          if ( _console == null )
            _console = CreatePlayerConsole();
        }

        return _console;
      }
    }

    protected virtual PlayerConsoleBase CreatePlayerConsole()
    {
      throw new InvalidOperationException();
    }

    public virtual void JoinedGame( GamePlayerBase player )
    {
    }

    public virtual bool TryQuitGame()
    {

      lock ( SyncRoot )
      {
        var player = GetPlayer();
        if ( player == null )
          return false;

        player.QuitGame();
        return true;
      }
    }

    public abstract GamePlayerBase GetPlayer();

    public abstract string[] Supports { get; }
  }
}
