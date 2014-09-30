using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ivony.Data;

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



    private PlayerHost( Guid guid )
    {
      Guid = guid;
      _console = new PlayerConsole( Guid );
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


    public static PlayerHost GetPlayerHost( Guid guid )
    {
      lock ( _sync )
      {
        return hosts[guid] as PlayerHost;
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



    private class PlayerConsole : PlayerConsoleBase
    {

      public Guid Guid { get; private set; }

      public PlayerConsole( Guid guid )
      {
        Guid = guid;
      }

      public override void WriteMessage( GameMessage message )
      {
        GameHost.Database
          .T( "INSERT INTO Messages ( Player, Type, Date, Message ) VALUES ( {...} ) ", Guid, message.Type, message.Date, message.Message )
          .ExecuteNonQuery();
      }
    }


  }
}
