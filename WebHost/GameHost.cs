using Ivony.Data;
using Ivony.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ivony.TableGame.Cards;
using System.Collections;

namespace Ivony.TableGame.WebHost
{
  public class GameHost
  {

    public static SqlDbExecutor Database = SqlServer.FromConfiguration( "Database" );


    private static object _sync = new object();
    private static Hashtable _games = new Hashtable();


    public static Game GetOrCreateGame( string name )
    {

      Game game;

      lock ( _sync )
      {

        game = _games[name] as Game;

        if ( game == null )
        {
          game = CreateGame( name );
          _games[name] = game;
        }
      }


      game.Initialize();
      return game;
    }

    private static Game CreateGame( string name )
    {
      return new TwelveCardsGame( name );
    }
  }
}