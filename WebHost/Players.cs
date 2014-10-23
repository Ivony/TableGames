using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class Players
  {

    private static object sync = new object();
    private static PlayerHostCollection players = new PlayerHostCollection();

    private static readonly TimeSpan timeout = new TimeSpan( 0, 10, 0 );


    /// <summary>
    /// 尝试获取玩家宿主
    /// </summary>
    /// <param name="id">玩家宿主ID</param>
    /// <returns></returns>
    public static PlayerHost GetPlayerHost( Guid id )
    {
      lock ( sync )
      {

        var date = DateTime.UtcNow - timeout;

        foreach ( var i in players.Where( item => item.RefreshTime < date ).ToArray() )
          i.Release();

        if ( players.Contains( id ) )
        {
          var instance = players[id];
          instance.RefreshState();
          return instance;
        }
        else
          return null;
      }
    }



    /// <summary>
    /// 创建一个新的玩家宿主
    /// </summary>
    /// <returns>新的玩家宿主</returns>
    public static PlayerHost CreatePlayerHost()
    {

      lock ( sync )
      {
        var instance = new PlayerHost( Guid.NewGuid(), PlayerNameManager.CreateName() );
        players.Add( instance );
        instance.ShowInitializeInfo();
        return instance;
      }
    }



    public static void Release( PlayerHost playerHost )
    {
      players.Remove( playerHost );
      PlayerNameManager.RemoveName( playerHost.Name );
    }
  }
}