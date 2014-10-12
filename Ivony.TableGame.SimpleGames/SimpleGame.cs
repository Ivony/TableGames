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
      dealer.RegisterCard( () => new CleanCard(), 40 );
      dealer.RegisterCard( () => new ShieldCard(), 20 );
      dealer.RegisterCard( () => new PeepCard(), 7 );
      dealer.RegisterCard( () => new AttackCard( 1 ), 50 );
      dealer.RegisterCard( () => new AttackCard( 2 ), 80 );
      dealer.RegisterCard( () => new AttackCard( 3 ), 50 );
      dealer.RegisterCard( () => new AttackCard( 4 ), 20 );
      dealer.RegisterCard( () => new AttackCard( 5 ), 10 );
      dealer.RegisterCard( () => new AttackCard( 6 ), 5 );
      dealer.RegisterCard( () => new AttackCard( 7 ), 3 );
      dealer.RegisterCard( () => new AttackCard( 8 ), 2 );
      dealer.RegisterCard( () => new AttackCard( 9 ), 1 );


      CardDealer = dealer;
    }



    public new SimpleGamePlayer[] Players
    {
      get { return base.Players.Cast<SimpleGamePlayer>().ToArray(); }
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

        if ( PlayerCollection.Count == 3 )
          gameHost.Run();

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



    protected override async Task RunGame()
    {

      AnnounceSystemMessage( "游戏开始" );

      int turn = 1;

      while ( true )
      {

        AnnounceSystemMessage( "第 {0} 回合", turn++ );


        AnnounceSystemMessage( "开始发牌" );
        DealCards();


        foreach ( SimpleGamePlayer player in Players )
        {
          await player.Play();

          if ( Players.Any( item => item.Health <= 0 ) )
            return;
        }
      }
    }
  }
}
