using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGame : StandardCardGame<SimpleGamePlayer, SimpleGameCard>
  {


    static SimpleGame()
    {

    }


    protected override GamePlayerBase TryJoinGameCore( IPlayerHost playerHost )
    {
      lock ( SyncRoot )
      {
        if ( PlayerCollection.Count == 3 )
          return null;


        var player = new SimpleGamePlayer( GameHost, playerHost );

        PlayerCollection.Add( player );

        if ( PlayerCollection.Count == 3 )
          GameHost.Run();

        return player;

      }
    }
  }
}
