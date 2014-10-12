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


    public SimpleGame Game
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


      DealCards();

      GameHost.Game.AnnounceMessage( "轮到 {0} 出牌", CodeName );
      PlayerHost.WriteMessage( "HP:{0} 卡牌:{1}", Health, string.Join( ", ", Cards.Select( item => item.Name ) ) );


      do
      {
        PlayCommand command = null;
        string commandText = null;

        try
        {
          commandText = await PlayerHost.Console.ReadLine( "请出牌" );
        }
        catch ( TaskCanceledException )
        {
          Game.AnnounceSystemMessage( "{0} 操作超时", CodeName );
          PlayerHost.WriteWarningMessage( "操作已超时，该回合不执行任何操作" );
          return;
        }


        try
        {
          command = ParseCommand( commandText );
        }
        catch ( FormatException )
        {
          PlayerHost.WriteMessage( "输入的命令格式错误" );
          continue;
        }


        if ( command != null )
          await command.Execute();


      } while ( false );

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

      if ( !int.TryParse( commandText, out cardIndex ) || cardIndex > 5 || cardIndex < 1 )
        throw new FormatException();


      return new PlayCommand( this, (SimpleGameCard) Cards[cardIndex - 1], Game.Players.Where( item => item != this ).ToArray().RandomItem() );
    }

    internal void RemoveCard( SimpleGameCard card )
    {
      CardCollection.Remove( card );
    }

    internal void RemoveAllCard()
    {
      CardCollection.RemoveAll( item => true );
    }

    /// <summary>
    /// 玩家当前是否有盾防效果
    /// </summary>
    public bool Shield { get; set; }

    public void DealCards()
    {
      CardCollection.AddRange( Game.CardDealer.DealCards( 5 - Cards.Length ) );
    }
  }
}
