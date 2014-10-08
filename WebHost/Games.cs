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


    public static IGameHost GetOrCreateGame( string name )
    {

      IGameHost game;

      lock ( _sync )
      {

        game = _games[name] as IGameHost;

        if ( game == null )
        {
          game = CreateGame( name );
          _games[name] = game;
        }
      }

      return game;
    }

    private static IGameHost CreateGame( string name )
    {
      var game = new SimpleGame( name );
      game.Initialize();
      return new WebGameHost<SimpleGame>( game );
    }
  }
}