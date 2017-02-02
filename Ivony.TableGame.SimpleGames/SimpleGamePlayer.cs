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
  public class SimpleGamePlayer : StandardCardGamePlayer<SimpleGameCard>
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
      HealthPoint = 15;

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
    protected override ICardCollection<SimpleGameCard> CardCollection { get { return _cards; } }




    protected override async Task PlayCard( CancellationToken token )
    {

      var confine = Effects.OfType<ConfineEffect>().FirstOrDefault();
      if ( confine != null )
      {
        Game.AnnounceMessage( $"只见 {PlayerName} 动弹不得，什么也做不了。" );
        PlayerHost.WriteMessage( "您被禁锢一回合，无法出牌" );
        Effects.RemoveEffect( confine );
        return;
      }

      ActionPoint = 1;
      DealCards();


      await base.PlayCard( token );
    }

    internal async Task PlayCard( ElementAttachmentCard card, CancellationToken token )
    {
      await card.Play( this, token );
      CardCollection.RemoveCard( card );
    }





    /// <summary>
    /// 重写 GetGameInformation 方法获取该玩家可以看到的游戏信息
    /// </summary>
    /// <returns>游戏信息对象</returns>
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


    /// <summary>
    /// 移除所有效果
    /// </summary>
    internal void Purify()
    {
      foreach ( var effect in Effects )
      {

        if ( Effects.RemoveEffect( effect ) )
          PlayerHost.WriteWarningMessage( "您当前的 {0} 效果已经被解除", effect.Name );

      }
    }


    /// <summary>
    /// 禁锢玩家一个回合
    /// </summary>
    internal void Confine()
    {
      PlayerHost.WriteWarningMessage( "您被禁锢一个回合" );
      SetEffect( new ConfineEffect() );
    }


    /// <summary>
    /// 重写 OnTimeout 方法处理玩家超时。
    /// </summary>
    /// <returns>用于等待处理完成的 Task 对象</returns>
    protected override Task OnTimeout()
    {
      Game.AnnounceMessage( $"{PlayerName} 面临严峻的局面，陷入了深深地沉思，以至于忘记了时间。天神见状想用一道惊雷唤醒，却将其雷了个外焦里嫩。" );

      HealthPoint -= 1;
      PlayerHost.WriteWarningMessage( $"操作超时，生命值减少 1 点，剩余生命值: {HealthPoint}" );
      return Task.CompletedTask;
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
