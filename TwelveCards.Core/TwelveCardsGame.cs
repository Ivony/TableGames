using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public class TwelveCardsGame : Game
  {

    public TwelveCardsGame()
    {

      cabins = new Cabin[10];
      for ( int i = 0; i < cabins.Length; i++ )
        cabins[i] = new Cabin( this, i );

    }



    private Cabin[] cabins;

    public override Cabin[] Cabins
    {
      get { return cabins; }
    }

    public override string Name
    {
      get { return "测试游戏"; }
    }


    protected override Player TryJoinGameCore( IPlayerHost host )
    {
      var candidates = Cabins.Where( item => item.Player == null ).ToArray();
      if ( candidates.Any() )
      {
        var cabin = candidates[Random.Next( candidates.Length )];
        return CreatePlayer( cabin, host );
      }

      else
        return null;
    }



    private Player CreatePlayer( Cabin cabin, IPlayerHost host )
    {
      var codeName = GetCodeName();
      var player = new Player( codeName, cabin, host );
      cabin.SetPlayer( player );
      return player;

    }

    private string GetCodeName()
    {
      return "扯犊子";
    }


  }
}
