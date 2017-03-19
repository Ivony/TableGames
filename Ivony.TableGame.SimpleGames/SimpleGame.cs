using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGame : StandardCardGame<SimpleGamePlayer, SimpleGameCard>
  {


    static SimpleGame()
    {

    }


    protected override async Task InitializeCore( IPlayerHost initializer )
    {

      var option = await initializer.Console.Choose( "请选择玩家数量：", new[] {
        Option.Create( Tuple.Create( 3 ), "三个玩家", "三个玩家的游戏" ),
        Option.Create( Tuple.Create( 5 ), "五个玩家", "五个玩家的游戏" ),
      }, CancellationToken.None );


      players = option.Item1;

      await base.InitializeCore( initializer );
    }


    private int players;

    protected override GamePlayerBase TryJoinGameCore( IPlayerHost playerHost )
    {
      lock ( SyncRoot )
      {
        if ( PlayerCollection.Count == players )
          return null;


        var player = new SimpleGamePlayer( GameHost, playerHost );

        PlayerCollection.Add( player );

        if ( PlayerCollection.Count == players )
          GameHost.Run();

        return player;

      }
    }
  }
}
