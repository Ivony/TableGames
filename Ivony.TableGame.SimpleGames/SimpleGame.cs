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
      var specialDealer = new UnlimitedCardDealer()
          .Register( () => new OverturnCard(), 1 )
          .Register( () => new AngelCard(), 2 )
          .Register( () => new DevilCard(), 5 )
          .Register( () => new ClearCard(), 10 )
          .Register( () => new PurifyCard(), 5 )
          .Register( () => new ReboundCard(), 3 )
          .Register( () => new ShieldCard(), 10 )
          .Register( () => new PeepCard(), 8 );

      var normalDealer = new UnlimitedCardDealer()
          .Register( () => new AttackCard( 1 ), 30 )
          .Register( () => new AttackCard( 2 ), 40 )
          .Register( () => new AttackCard( 3 ), 50 )
          .Register( () => new AttackCard( 4 ), 25 )
          .Register( () => new AttackCard( 5 ), 20 )
          .Register( () => new AttackCard( 6 ), 15 )
          .Register( () => new AttackCard( 7 ), 10 )
          .Register( () => new AttackCard( 8 ), 7 )
          .Register( () => new AttackCard( 9 ), 5 )
          .Register( () => new AttackCard( 10 ), 3 );

      return new UnlimitedCardDealer()
          .Register( () => specialDealer.DealCard(), 4 )
          .Register( () => normalDealer.DealCard(), 6 );
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
