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
  public class SimpleGamePlayer : CardGamePlayer
  {


    /// <summary>
    /// 创建 SimpleGamePlayer 对象
    /// </summary>
    /// <param name="gameHost">游戏宿主</param>
    /// <param name="playerHost">玩家宿主</param>
    public SimpleGamePlayer( IGameHost gameHost, IPlayerHost playerHost )
      : base( gameHost, playerHost )
    {
      Game = (SimpleGame) gameHost.Game;
      HealthPoint = 25;

    }


    /// <summary>
    /// 获取当前参加的游戏对象
    /// </summary>
    public new SimpleGame Game
    {
      get;
      private set;
    }



    private SimpleGameCardCollection _cards = new SimpleGameCardCollection();
    /// <summary>
    /// 重写此属性自定义卡牌集
    /// </summary>
    protected override ICardCollection CardCollection { get { return _cards; } }





    /// <summary>
    /// 重写 OnBeforePlay ，在出牌前处理效果和显示状态信息
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns>一个 Task 对象，可用于等待操作完成</returns>
    protected override async Task OnBeforePlayCard( CancellationToken token )
    {
      DealCards();


      foreach ( var effect in Effects.OfType<IAroundEffect>() )
        await effect.OnTurnedAround( this );


      PlayerHost.WriteMessage( "HP:{0,-3}{1} 卡牌:{2}", HealthPoint, Effects, string.Join( ", ", Cards.Select( item => item.Name ) ) );
    }





    protected override async Task PlayCard( CancellationToken token )
    {
      var card =await CherryCard( token );
      await ((SimpleGameCard) card).UseCard( this, Game.Players.Where( item => item != this ).ToArray().RandomItem() );
      CardCollection.RemoveCard( card );
    }





    public override object GetGameInformation()
    {
      return new
      {
        Players = Game.Players.Select( item => item.PlayerName ),
        Cards = CardCollection,
      };
    }





    /// <summary>
    /// 给玩家发牌
    /// </summary>
    public void DealCards()
    {
      _cards.DealCards();
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
      CardCollection.Clear();
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
