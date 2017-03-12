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
using System.Collections.Concurrent;

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
    /// 尝试附加一种诅咒效果
    /// </summary>
    /// <param name="curseEffect">诅咒效果</param>
    /// <returns></returns>
    internal bool Curse( SimpleGamePlayer player, CurseEffect curseEffect )
    {
      if ( Effects.OfType<ShieldEffect>().FirstOrDefault( item => item.Element == Element.水 ) != null )
        return false;

      if ( Effects.OfType<BlessEffect>().FirstOrDefault( item => item.Element == Element.水 ) != null )
      {
        if ( player != null )
          player.Curse( null, curseEffect );
        return false;
      }

      SetEffect( curseEffect );
      return true;
    }

    /// <summary>
    /// 与另一个玩家交换所有卡牌
    /// </summary>
    /// <param name="target"></param>
    internal void ExchangeCards( SimpleGamePlayer target )
    {
      DealCards();
      target.DealCards();

      var cards = target._cards;
      target._cards = cards;
      _cards = cards;

      PlayerHost.WriteMessage( "卡牌已经交换" );

    }


    /// <summary>
    /// 重置手上所有卡牌
    /// </summary>
    internal void ResetCards()
    {
      CardCollection.Clear();
      DealCards();
      NotifyCardsHasBeenReset();
    }


    /// <summary>
    /// 将手上所有卡牌盾牌换成攻击，攻击换成盾牌
    /// </summary>
    internal void ReflexiveCards()
    {
      throw new NotImplementedException();
      //((SimpleGameCardCollection) CardCollection).ReflexiveCards();
    }





    private void NotifyCardsHasBeenReset()
    {
      WriteSecretMessage( GameMessage.Warning( "一个魔术师借了你所有卡牌表演魔术，一阵闪光过后你的卡牌和魔术师都消失了，空中飘下来几张你没见过的卡牌。" ) );
    }

    /// <summary>
    /// 丢弃指定的卡牌
    /// </summary>
    /// <param name="predicate">确定卡牌是不是要丢弃的方法</param>
    internal void DiscardCards( Func<SimpleGameCard, bool> predicate )
    {
      foreach ( var card in Cards.Where( predicate ).ToArray() )
        CardCollection.RemoveCard( card );

      DealCards();
    }

    /// <summary>
    /// 实现卡牌被盗窃的方法
    /// </summary>
    /// <param name="user">盗窃者</param>
    /// <param name="token">取消标识</param>
    /// <returns>用于等待的 Task 对象</returns>
    internal async Task StealCardBy( SimpleGamePlayer user, CancellationToken token )
    {
      DealCards();

      var options = Cards.Select( item => Option.Create( item, item.Name, item.Description ) ).ToArray();

      var card = await user.PlayerHost.Console.Choose( "请选择要盗取的卡牌：", options, null, token );
      if ( card == null )
        throw new TimeoutException();

      CardCollection.RemoveCard( card );
      WriteSecretMessage( GameMessage.Warning( $"一天深夜，你回到家，发现被你藏在绝密地方的 {card.Name} 卡牌被人偷走了。" ) );
      user.CardCollection.AddCard( card );
      user.PlayerHost.WriteMessage( $"您偷到了一张 {card.Name} 卡牌" );
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


    private ConcurrentBag<GameMessage> secretMessages = new ConcurrentBag<GameMessage>();

    /// <summary>
    /// 写入一条秘密消息，轮到玩家出牌时才能看到。
    /// </summary>
    /// <param name="message">秘密消息</param>
    public void WriteSecretMessage( GameMessage message )
    {
      secretMessages.Add( message );
    }


    /// <summary>
    /// 重写此方法进行游戏
    /// </summary>
    /// <param name="roundEvent">玩家回合事件信息</param>
    /// <param name="token">取消标识</param>
    /// <returns>用于等待玩家操作完成的 Task 对象</returns>
    protected override async Task PlayCard( PlayerRoundEvent roundEvent, CancellationToken token )
    {
      {
        var list = secretMessages;
        secretMessages = new ConcurrentBag<GameMessage>();
        foreach ( var message in list )
          PlayerHost.WriteMessage( message, true );
      }

      if ( (bool?) roundEvent.DataBag.Confine == true )
      {
        Game.AnnounceMessage( $"只见 {PlayerName} 动弹不得，什么也做不了。" );
        PlayerHost.WriteMessage( "您被禁锢一回合，无法出牌" );
        return;
      }

      ActionPoint = 1;
      DealCards();


      await base.PlayCard( roundEvent, token );
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
    /// <param name="positive">是否只移除正面效果，还是只移除负面效果，或者都移除</param>
    internal void Purify( bool? positive = null )
    {
      foreach ( var effect in Effects )
      {
        if ( positive == false && effect is IPositiveEffect )
          continue;

        if ( positive == true && effect is INagativeEffect )
          continue;

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
      Game.AnnounceMessage( $"{PlayerName} 面临严峻的局面，陷入了深深地沉思，忽然天空中出现了一道惊雷，将其雷了个外焦里嫩。" );

      HealthPoint -= 1;
      PlayerHost.WriteWarningMessage( $"操作超时，生命值减少 1 点，剩余生命值: {HealthPoint}" );
      return Task.CompletedTask;
    }



    private SimpleGamePlayerEffectCollection _effects = new SimpleGamePlayerEffectCollection();

    /// <summary>
    /// 获取当前玩家所有效果
    /// </summary>
    public override IEffectCollection Effects
    {
      get { return _effects; }
    }


    /// <summary>
    /// 为玩家设置一个效果
    /// </summary>
    /// <param name="effect"></param>
    internal void SetEffect( SimpleGameEffect effect )
    {
      Effects.AddEffect( effect );
    }


    protected override IOption<SimpleGameCard> CreateOption( SimpleGameCard card )
    {
      if ( card == null )
        return null;

      var option = new Option( card.Name, card.Description, card.Availables( this ) == false );
      return Option.Create( card, option );
    }
  }
}
