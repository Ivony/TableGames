using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ivony.TableGame;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGamePlayer : GamePlayer
  {

    public SimpleGamePlayer( string codeName, IGameHost gameHost, IPlayerHost playerHost )
      : base( codeName, gameHost, playerHost )
    {
      Game = (SimpleGame) gameHost.Game;
      Health = 100;

    }


    protected SimpleGame Game
    {
      get;
      private set;
    }



    /// <summary>
    /// 生命值
    /// </summary>
    public int Health
    {
      get;
      internal set;
    }


    public async Task Play()
    {

    begin:

      try
      {
        var command = ParseCommand( await PlayerHost.Console.ReadLine( "请出牌" ) );

        if ( command != null )
          await command.Execute();


      }
      catch ( FormatException e )
      {
        PlayerHost.WriteMessage( "输入的命令格式错误" );
        goto begin;
      }

    }



    public override object GetGameInformation()
    {
      return new
      {
        Players = Game.Players.ToDictionary( item => item.CodeName, item => item.Health ),
        Cards = Cards,
      };
    }





    private PlayCommand ParseCommand( string commandText )
    {

      int cardIndex;

      if ( !int.TryParse( commandText, out cardIndex ) || cardIndex > 5 )
        throw new FormatException();


      return new PlayCommand( this, (SimpleGameCard) Cards[cardIndex], Game.Players.Where( item => item != this ).ToArray().RandomItem() );


    }

    internal void RemoveCard( SimpleGameCard card )
    {
      CardCollection.Remove( card );
    }
  }
}
