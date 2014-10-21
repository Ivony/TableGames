using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ivony.TableGame;
using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;
using System.Threading;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGamePlayer : CardGamePlayer<SimpleGameCard>
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


    /// <summary>
    /// 重写 OnBeforePlay ，在出牌前处理效果和显示状态信息
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns>一个 Task 对象，可用于等待操作完成</returns>
    protected override async Task OnBeforePlay( CancellationToken token )
    {
      DealCards();


      foreach ( var effect in Effects.OfType<IAroundEffect>() )
        await effect.OnTurnedAround( this );


      PlayerHost.WriteMessage( "HP:{0,-3}{1} 卡牌:{2}", HealthPoint, Effects, string.Join( ", ", Cards.Select( item => item.Name ) ) );
    }


    protected override async Task PlayCard( SimpleGameCard card, CancellationToken token )
    {
      await card.UseCard( this, Game.Players.Where( item => item != this ).ToArray().RandomItem() );
      CardCollection.Remove( card );
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
      DealCards( 6 - Cards.Length );
    }

    internal void Purify()
    {
      foreach ( var effect in Effects )
      {

        if ( Effects.RemoveEffect( effect ) )
          PlayerHost.WriteWarningMessage( "您当前的 {0} 效果已经被解除", effect.Name );

      }
    }

    internal void ClearCards()
    {
      CardCollection.RemoveAll( item => true );
      PlayerHost.WriteMessage( "您手上的卡牌已经清空，请等待下次发牌" );
    }



    private SimpleGamePlayerEffectCollection _effects = new SimpleGamePlayerEffectCollection();

    public override IEffectCollection Effects
    {
      get { return _effects; }
    }


    internal void SetEffect( SimpleGameEffect effect )
    {
      Effects.AddEffect( effect );
    }


  }
}
