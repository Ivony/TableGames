using Ivony.Data;
using Ivony.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Ivony.TableGame.SimpleGames;

namespace Ivony.TableGame.WebHost
{
  public class Games
  {

    public static SqlDbExecutor Database = SqlServer.FromConfiguration( "Database" );


    private static object _sync = new object();
    private static Hashtable _games = new Hashtable();


    /// <summary>
    /// 获取或创建一个游戏
    /// </summary>
    /// <param name="name">游戏名称</param>
    /// <returns></returns>
    public static WebGameHost GetOrCreateGame( string name )
    {

      WebGameHost game;

      lock ( _sync )
      {

        game = _games[name] as WebGameHost;

        if ( game == null )
        {
          game = CreateGame( name );
          _games[name] = game;
        }
      }

      return game;
    }

    private static WebGameHost CreateGame( string name )
    {
      return new WebGameHost( name );
    }
  }
}