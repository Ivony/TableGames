using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ivony.TableGame;
using Ivony.TableGame.Basics;
using Ivony.TableGame.SimpleGames.Rules;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGamePlayer : BasicGamePlayer<SimpleGameCard>
  {

    public SimpleGamePlayer( string codeName, IGameHost gameHost, IPlayerHost playerHost )
      : base( codeName, gameHost, playerHost )
    {
      Game = (SimpleGame) gameHost.Game;
      HealthPoint = 100;

    }


    public SimpleGame Game
    {
      get;
      private set;
    }


    protected override async Task OnBeforePlay()
    {
      DealCards();

      var devil = SpecialEffect as DevilEffect;
      if ( devil != null )
        devil.Win( this );


      PlayerHost.WriteMessage( "HP:{0,-3}{1}{2} 卡牌:{3}", HealthPoint, DefenceEffect, SpecialEffect, string.Join( ", ", Cards.Select( item => item.Name ) ) );
    }


    protected override async Task PlayCard( SimpleGameCard card )
    {
      await card.UseCard( this, Game.Players.Where( item => item != this ).ToArray().RandomItem() );
      CardCollection.Remove( card );
    }


    public ISpecialEffect SpecialEffect
    {
      get;
      set;
    }


    public IDefenceEffect DefenceEffect
    {
      get;
      set;
    }



    public override object GetGameInformation()
    {
      return new
      {
        Players = Game.Players.ToDictionary( item => item.CodeName, item => item.HealthPoint ),
        Cards = Cards,
      };
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
    /// 给玩家发牌
    /// </summary>
    public void DealCards()
    {
      DealCards( 5 - Cards.Length );
    }
  }
}
