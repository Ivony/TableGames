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


    public SimpleGame( IGameHost gameHost )
      : base( gameHost )
    {
    }


    static SimpleGame()
    {

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
