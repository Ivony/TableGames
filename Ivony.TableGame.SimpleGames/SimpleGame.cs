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


    public SimpleGame( string name ) : base( name ) { }


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


    protected override GameProgress StartGameCore()
    {
      return new SimpleGameProgress( this );
    }

  }
}
