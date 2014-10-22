using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGame : CardGame<SimpleGamePlayer>
  {


    public SimpleGame( IGameHost gameHost )
      : base( gameHost )
    {
    }


    public static CardDealer<SimpleGameCard> SpecialCardDealer { get; private set; }
    public static CardDealer<SimpleGameCard> AttackCardDealer { get; private set; }


    static SimpleGame()
    {

      SpecialCardDealer = new UnlimitedCardDealer<SimpleGameCard>()
          .Register( () => new OverturnCard(), 1 )
          .Register( () => new AngelCard(), 2 )
          .Register( () => new DevilCard(), 5 )
          .Register( () => new ClearCard(), 10 )
          .Register( () => new PurifyCard(), 5 )
          .Register( () => new ReboundCard(), 3 )
          .Register( () => new ShieldCard(), 10 )
          .Register( () => new PeepCard(), 8 );

      AttackCardDealer = new UnlimitedCardDealer<SimpleGameCard>()
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
  }
}
