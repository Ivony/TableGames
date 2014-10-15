using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ivony.TableGame;
using Ivony.TableGame.Basics;
using Ivony.TableGame.SimpleGames.Rules;
using System.Threading;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGamePlayer : BasicGamePlayer<SimpleGameCard>
  {

    public SimpleGamePlayer( IGameHost gameHost, IPlayerHost playerHost )
      : base( gameHost, playerHost )
    {
      Game = (SimpleGame) gameHost.Game;
      HealthPoint = 25;

    }


    public SimpleGame Game
    {
      get;
      private set;
    }


    protected override async Task OnBeforePlay( CancellationToken token )
    {
      DealCards();

      {
        var effect = SpecialEffect as IAroundEffect;
        if ( effect != null )
          effect.OnTurnedAround( this );
      }

      {
        var effect = DefenceEffect as IAroundEffect;
        if ( effect != null )
          effect.OnTurnedAround( this );
      }

      PlayerHost.WriteMessage( "HP:{0,-3}{1}{2} 卡牌:{3}", HealthPoint, DefenceEffect, SpecialEffect, string.Join( ", ", Cards.Select( item => item.Name ) ) );
    }


    protected override async Task PlayCard( SimpleGameCard card, CancellationToken token )
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
        Players = Game.Players.Select( item => item.PlayerName ),
        Cards = Cards,
      };
    }







    /// <summary>
    /// 给玩家发牌
    /// </summary>
    public void DealCards()
    {
      DealCards( 5 - Cards.Length );
    }

    internal void Purify()
    {
      if ( SpecialEffect != null )
      {
        PlayerHost.WriteWarningMessage( "您当前的 {0} 效果已经被解除", SpecialEffect.Name );
        SpecialEffect = null;
      }

      if ( DefenceEffect != null )
      {
        PlayerHost.WriteWarningMessage( "您当前的 {0} 效果已经被解除", DefenceEffect.Name );
        DefenceEffect = null;
      }

    }

    internal void ClearCards()
    {
      CardCollection.RemoveAll( item => true );
      PlayerHost.WriteMessage( "您手上的卡牌已经清空，请等待下次发牌" );
    }
  }
}
