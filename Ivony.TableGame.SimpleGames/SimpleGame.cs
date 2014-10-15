using Ivony.TableGame.Basics;
using Ivony.TableGame.SimpleGames.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGame : BasicGame<SimpleGamePlayer, SimpleGameCard>
  {



    private string[] names = new[] { "张三", "李四", "王五" };


    public SimpleGame( IGameHost gameHost ) : base( gameHost ) { }


    protected override CardDealer CreateCardDealer()
    {
      var dealer = new UnlimitedCardDealer();
      dealer.RegisterCard( () => new OverturnCard(), 15);
      dealer.RegisterCard( () => new AngelCard(), 40 );
      dealer.RegisterCard( () => new DevilCard(), 200 );
      dealer.RegisterCard( () => new ClearCard(), 250 );
      dealer.RegisterCard( () => new PurifyCard(), 100 );
      dealer.RegisterCard( () => new ReboundCard(), 20 );
      dealer.RegisterCard( () => new ShieldCard(), 200 );
      dealer.RegisterCard( () => new PeepCard(), 120 );
      dealer.RegisterCard( () => new AttackCard( 1 ), 400 );
      dealer.RegisterCard( () => new AttackCard( 2 ), 600 );
      dealer.RegisterCard( () => new AttackCard( 3 ), 500 );
      dealer.RegisterCard( () => new AttackCard( 4 ), 300 );
      dealer.RegisterCard( () => new AttackCard( 5 ), 200 );
      dealer.RegisterCard( () => new AttackCard( 6 ), 150 );
      dealer.RegisterCard( () => new AttackCard( 7 ), 100 );
      dealer.RegisterCard( () => new AttackCard( 8 ), 80 );
      dealer.RegisterCard( () => new AttackCard( 9 ), 50 );
      dealer.RegisterCard( () => new AttackCard( 10 ), 20 );

      return dealer;
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
  }
}
