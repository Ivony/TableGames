using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ivony.Data;
using System.Web.Http.ModelBinding;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Ivony.TableGame.WebHost
{


  /// <summary>
  /// 玩家宿主，登陆用户在系统中的宿主对象
  /// </summary>
  public class PlayerHost : IPlayerHost
  {


    public Guid Guid
    {
      get;
      private set;
    }



    private PlayerHost( Guid id )
    {
      Guid = id;
      _console = new PlayerConsole( this );
    }


    public static PlayerHost CreatePlayerHost()
    {

      lock ( _sync )
      {

        var host = new PlayerHost( Guid.NewGuid() );
        hosts.Add( host.Guid, host );
        return host;

      }
    }

    private static object _sync = new object();
    private static Hashtable hosts = new Hashtable();


    public static PlayerHost GetPlayerHost( Guid userId )
    {
      lock ( _sync )
      {
        return hosts[userId] as PlayerHost;
      }

    }


    private PlayerConsole _console;

    /// <summary>
    /// 获取玩家控制台，用于给玩家显示消息
    /// </summary>
    public PlayerConsoleBase Console
    {
      get { return _console; }
    }



    /// <summary>
    /// 若已经加入某个游戏，则获取游戏中的玩家对象
    /// </summary>
    public GamePlayer Player { get; private set; }


    public GamePlayer GetPlayer()
    {
      return Player;
    }

    /// <summary>
    /// 玩家已经加入游戏
    /// </summary>
    /// <param name="player"></param>
    public void JoinedGame( GamePlayer player )
    {

      lock ( _sync )
      {
        if ( Player != null )
          throw new InvalidOperationException( "玩家当前已经在另一个游戏，无法加入游戏" );

        Player = player;
      }
    }


    /// <summary>
    /// 玩家已经从游戏中释放
    /// </summary>
    public void LeavedGame()
    {
      lock ( _sync )
      {
        Player = null;
      }
    }





    private class PlayerConsole : PlayerConsoleBase
    {

      public PlayerHost PlayerHost { get; private set; }

      public PlayerConsole( PlayerHost host )
      {
        PlayerHost = host;
      }

      public override void WriteMessage( GameMessage message )
      {
        PlayerHost._messages.Add( message );
      }

      public override string ReadLine( string prompt )
      {
        throw new NotImplementedException();
      }
    }


    private List<GameMessage> _messages = new List<GameMessage>();

    public GameMessage[] GetMessages( long timeStamp = 0 )
    {

      return _messages.SkipWhile( item => item.Date.Ticks <= timeStamp ).ToArray();
    }



    public override string ToString()
    {
      return Guid.ToString();
    }

  }




}
