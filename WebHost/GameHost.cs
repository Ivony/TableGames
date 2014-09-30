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


    public void Initialize( int cabins )
    {
      var cardDealer = new CardDealer();

      cardDealer.RegisterCard( () => new NormalAttackCard( 1 ), 100 );
      cardDealer.RegisterCard( () => new NormalAttackCard( 2 ), 70 );
      cardDealer.RegisterCard( () => new NormalAttackCard( 3 ), 50 );
      cardDealer.RegisterCard( () => new NormalAttackCard( 4 ), 20 );
      cardDealer.RegisterCard( () => new NormalAttackCard( 5 ), 10 );

      Game = new Game( cardDealer, cabins );
    }


    public void AddPlayer( PlayerHost player )
    {

    }



  }
}