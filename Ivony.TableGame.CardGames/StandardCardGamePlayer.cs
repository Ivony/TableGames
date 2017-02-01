using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 提供一个通用标准的卡牌用户实现
  /// </summary>
  public abstract class StandardCardGamePlayer<TCard> : CardGamePlayer, IGameEventListener where TCard : StandardCard
  {


    protected StandardCardGamePlayer( IGameHost gameHost, IPlayerHost playerHost ) :
      base( gameHost, playerHost )
    {


    }




    /// <summary>
    /// 获取或设置生命点数（若在回合结束后，某玩家的生命点数小于等于 0 ，该玩家将退出游戏）
    /// </summary>
    public int HealthPoint { get; set; }


    /// <summary>
    /// 获取或设置行动点数（若行动力点数小于目前所要执行的行动点数，则不能行动）
    /// </summary>
    public int ActionPoint { get; set; }






    protected virtual ICardCollection<TCard> CardCollection { get; } = new CardCollection<TCard>();


    /// <summary>
    /// 获取标准卡牌组
    /// </summary>
    public TCard[] Cards
    {
      get { return CardCollection.Cast<TCard>().ToArray(); }
    }




    /// <summary>
    /// 实现 PlayCard 方法，令玩家出牌
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns>返回一个 Task 对象，用于等待玩家出牌完毕</returns>
    protected override async Task PlayCard( CancellationToken token )
    {

      while ( ActionPoint > 0 )
      {
        var card = await CherryCard( token );
        if ( card == null )
          return;


        await card.Play( this, token );

        CardCollection.RemoveCard( card );
        ActionPoint -= card.ActionPoint;
      }
    }



    /// <summary>
    /// 选取一张卡牌
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns></returns>
    protected virtual async Task<TCard> CherryCard( CancellationToken token )
    {

      var options = CreateOptions( Cards );

      if ( options.All( item => item.OptionDescriptor.Disabled ) )//如果所有卡牌都不可用，则此回合不能再行动
        return null;

      var card = await PlayerHost.Console.Choose( "请出牌：", options, null, token );

      if ( card != null )
        return card;


      lock ( SyncRoot )
      {
        var index = Random.Next( Cards.Length );
        PlayerHost.WriteWarningMessage( "操作超时，随机打出第 {0} 张牌", index + 1 );
        return Cards[index];
      }
    }


    /// <summary>
    /// 为卡牌组创建选项组
    /// </summary>
    /// <param name="cards">卡牌组</param>
    /// <returns>选项组</returns>
    protected virtual IOption<TCard>[] CreateOptions( TCard[] cards )
    {
      return cards.Select( item => CreateOption( item ) ).Where( item => item != null ).ToArray();
    }

    /// <summary>
    /// 为卡牌创建选项对象
    /// </summary>
    /// <param name="card">要创建选项对象的卡牌</param>
    /// <returns>选项对象</returns>
    protected virtual IOption<TCard> CreateOption( TCard card )
    {

      if ( card == null )
        return null;


      var disabled = card.ActionPoint > ActionPoint;
      var name = card.Name;

      if ( disabled )
        name = "*" + name;


      return Option.Create( card, name, card.Description, disabled );
    }







    /// <summary>
    /// 确认玩家目前还活着，若已经死亡，则调用 OnDead 方法
    /// </summary>
    /// <returns></returns>
    public virtual Task EnsureAlive()
    {
      if ( this.HealthPoint <= 0 )
        return OnDead();

      else
        return Task.CompletedTask;
    }



    /// <summary>
    /// 当玩家死亡时调用此方法。
    /// </summary>
    public virtual Task OnDead()
    {
      return Task.Run( () =>
        {
          Game.AnnounceSystemMessage( "玩家 {0} 已经阵亡", PlayerName );
          QuitGame();
        } );
    }



    IEffectCollection _effects = new NotSupportEffectCollection();
    /// <summary>
    /// 获取玩家目前所有效果的集合
    /// </summary>
    public virtual IEffectCollection Effects
    {
      get { return _effects; }
    }


    /// <summary>
    /// 处理游戏中发生的事件
    /// </summary>
    /// <param name="gameEvent">游戏事件</param>
    /// <returns>用于等待事件处理完成的 Task</returns>
    public override Task OnGameEvent( IGameEvent gameEvent )
    {
      var behaviorEvent = gameEvent as IGameBehaviorEvent;
      if ( behaviorEvent != null )
        return OnGameEvent( behaviorEvent );


      var playerEvent = gameEvent as IGamePlayerEvent;
      if ( playerEvent != null )
        return OnGameEvent( playerEvent );


      return Task.CompletedTask;
    }


    /// <summary>
    /// 处理玩家事件
    /// </summary>
    /// <param name="playerEvent">玩家事件/param>
    /// <returns>用于等待事件处理完成的 Task</returns>
    protected virtual async Task OnGameEvent( IGamePlayerEvent playerEvent )
    {
      if ( playerEvent.Player.Equals( this ) )
      {
        foreach ( var item in Effects.OfType<IGamePlayerEffect>() )
          await item.OnGamePlayerEvent( playerEvent );
      }
    }


    /// <summary>
    /// 处理玩家行为事件
    /// </summary>
    /// <param name="behaviorEvent">玩家行为事件/param>
    /// <returns>用于等待事件处理完成的 Task</returns>
    protected virtual async Task OnGameEvent( IGameBehaviorEvent behaviorEvent )
    {

      if ( behaviorEvent.InitiatePlayer.Equals( this ) )
      {
        foreach ( var item in Effects.OfType<IGameBehaviorEffect>() )
          await item.OnBehaviorInitiator( behaviorEvent );
      }

      if ( behaviorEvent.RecipientPlayer.Equals( this ) )
      {
        foreach ( var item in Effects.OfType<IGameBehaviorEffect>() )
          await item.OnBehaviorRecipient( behaviorEvent );
      }
    }
  }
}
