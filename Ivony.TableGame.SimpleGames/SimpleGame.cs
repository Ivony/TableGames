using Ivony.TableGame.BasicCardGames;
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


    public SimpleGame( IGameHost gameHost )
      : base( gameHost )
    {
      EffectManager = new SimpleGameEffectManager( this );
    }


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
          .Register( () => specialDealer.DealCard(), 3 )
          .Register( () => normalDealer.DealCard(), 7 );
    }


    protected override GamePlayerBase TryJoinGameCore( IGameHost gameHost, IPlayerHost playerHost )
    {
      lock ( SyncRoot )
      {
        if ( PlayerCollection.Count == 3 )
          return null;


        var player = new SimpleGamePlayer( gameHost, playerHost );

        PlayerCollection.Add( player );

        if ( PlayerCollection.Count == 3 )
          gameHost.Run();

        return player;

      }
    }


    protected SimpleGameEffectManager EffectManager { get; private set; }


    internal void SetPlayerEffect( SimpleGamePlayer player, SimpleGameEffect effect )
    {
      if ( !EffectManager.TryAddEffect( player, effect ) )
        throw new InvalidOperationException();
    }
  }
}
