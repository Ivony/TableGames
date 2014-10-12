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
      dealer.RegisterCard( () => new AngelCard(), 3 );
      dealer.RegisterCard( () => new DevilCard(), 25 );
      dealer.RegisterCard( () => new CleanCard(), 40 );
      dealer.RegisterCard( () => new ShieldCard(), 20 );
      dealer.RegisterCard( () => new PeepCard(), 15 );
      dealer.RegisterCard( () => new AttackCard( 1 ), 50 );
      dealer.RegisterCard( () => new AttackCard( 2 ), 80 );
      dealer.RegisterCard( () => new AttackCard( 3 ), 50 );
      dealer.RegisterCard( () => new AttackCard( 4 ), 20 );
      dealer.RegisterCard( () => new AttackCard( 5 ), 10 );
      dealer.RegisterCard( () => new AttackCard( 6 ), 7 );
      dealer.RegisterCard( () => new AttackCard( 7 ), 5 );
      dealer.RegisterCard( () => new AttackCard( 8 ), 3 );
      dealer.RegisterCard( () => new AttackCard( 9 ), 2 );
      dealer.RegisterCard( () => new AttackCard( 10 ), 1 );


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




    protected internal CardDealer CardDealer { get; private set; }



    protected override async Task RunGame()
    {

      AnnounceSystemMessage( "游戏开始" );

      int turn = 1;

      while ( true )
      {

        AnnounceSystemMessage( "第 {0} 回合", turn++ );


        AnnounceSystemMessage( "开始发牌" );


        foreach ( SimpleGamePlayer player in Players )
        {
          await player.Play();

          var dead = Players.FirstOrDefault( item => item.Health <= 0 );
          if ( dead != null )
          {
            AnnounceSystemMessage( "玩家 {0} 已经阵亡，游戏结束", dead.CodeName );
            return;
          }
        }
      }
    }
  }
}
