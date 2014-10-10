using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGame : Game
  {



    private string[] names = new[] { "张三", "李四", "王五" };


    public SimpleGame( string name )
      : base( name )
    {

      var dealer = new UnlimitedCardDealer();
      dealer.RegisterCard( () => new AttackCard(), 10 );
      dealer.RegisterBlankCard( 10 );


      CardDealer = dealer;
    }


    protected override GamePlayer TryJoinGameCore( IGameHost gameHost, IPlayerHost playerHost )
    {
      lock ( SyncRoot )
      {
        if ( PlayerCollection.Count == 3 )
          return null;


        var name = names[PlayerCollection.Count];

        var player = new SimpleGamePlayer( name, gameHost, playerHost );

        PlayerCollection.Add( player );
        return player;

      }
    }




    protected CardDealer CardDealer { get; private set; }


    internal void DealCards()
    {
      EnsureGameRunning();


      foreach ( var player in Players )
      {
        player.AddCard( CardDealer.DealCards( 5 - player.Cards.Length ) );
      }

    }


  }
}
