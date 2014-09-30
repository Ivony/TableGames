using Ivony.Data;
using Ivony.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwelveCards.Cards;

namespace TwelveCards.WebHost
{
  public class GameHost
  {

    public Game Game
    {
      get;
      private set;
    }


    public void Initialize(  )
    {

      Game = new TwelveCardsGame();
      Game.Initialize();
    }


    public Player TryJoinGame( PlayerHost host )
    {
      return Game.TryJoinGame( host );
    }





    public static SqlDbExecutor Database = SqlServer.FromConfiguration( "Database" );

  }
}