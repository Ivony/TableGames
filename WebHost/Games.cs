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

    private static object _sync = new object();
    private static GameHostCollection _games = new GameHostCollection();


    /// <summary>
    /// 获取或创建一个游戏
    /// </summary>
    /// <param name="name">游戏名称</param>
    /// <returns></returns>
    public static GameHost GetOrCreateGame( string name )
    {


      lock ( _sync )
      {
        if ( _games.Contains( name ) )
          return _games[name];

        else
        {
          var game = new GameHost( name );
          _games.Add( game );
          return game;
        }
      }
    }

    internal static void ReleaseGameHost( GameHost gameHost )
    {
      lock ( _sync )
      {
        _games.Remove( gameHost );
      }
    }



  }
}