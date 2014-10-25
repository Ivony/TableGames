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
  public abstract class StandardCardGamePlayer : CardGamePlayer, IGameEventListener
  {


    protected StandardCardGamePlayer( IGameHost gameHost, IPlayerHost playerHost ) : base( gameHost, playerHost ) { }




    /// <summary>
    /// 获取或设置生命点数（若在回合结束后，某玩家的生命点数小于等于 0 ，该玩家将退出游戏）
    /// </summary>
    public int HealthPoint { get; set; }


    /// <summary>
    /// 获取或设置行动点数（若行动力点数小于目前所要执行的行动点数，则不能行动）
    /// </summary>
    public int ActionPoint { get; set; }



    /// <summary>
    /// 获取标准卡牌组
    /// </summary>
    protected StandardCard[] StandardCards
    {
      get { return Cards.Cast<StandardCard>().ToArray(); }
    }


    /// <summary>
    /// 实现 PlayCard 方法，令玩家出牌
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns>返回一个 Task 对象，用于等待玩家出牌完毕</returns>
    protected override async Task PlayCard( CancellationToken token )
    {

      while ( true )
      {
        var card = await CherryCard( token );
        if ( card == null )
        {
          break;
        }

        await card.Play( this, CherryTarget( card.TargetType, token ), token );

        CardCollection.RemoveCard( card );
        ActionPoint -= card.ActionPoint;
      }
    }



    /// <summary>
    /// 选取一张卡牌
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns></returns>
    protected virtual async Task<StandardCard> CherryCard( CancellationToken token )
    {

      var options = CreateOptions( StandardCards );

      if ( options.All( item => item.Disabled ) )//如果所有卡牌都不可用，则此回合不能再行动
        return null;

      var card = await PlayerHost.Console.Choose( "请出牌：", options, null, token );

      if ( card != null )
        return card;


      lock ( SyncRoot )
      {
        var index = Random.Next( StandardCards.Length );
        PlayerHost.WriteWarningMessage( "操作超时，随机打出第 {0} 张牌", index + 1 );
        return StandardCards[index];
      }
    }


    /// <summary>
    /// 为卡牌组创建选项组
    /// </summary>
    /// <param name="cards">卡牌组</param>
    /// <returns>选项组</returns>
    protected virtual Option<StandardCard>[] CreateOptions( StandardCard[] cards )
    {
      return cards.Select( item => CreateOption( item ) ).ToArray();
    }

    /// <summary>
    /// 为卡牌创建选项对象
    /// </summary>
    /// <param name="card">要创建选项对象的卡牌</param>
    /// <returns>选项对象</returns>
    protected virtual Option<StandardCard> CreateOption( StandardCard card )
    {

      var disabled = card.ActionPoint > ActionPoint;
      var name = card.Name;

      if ( disabled )
        name = "*" + name;


      return new Option<StandardCard>( card, name, card.Description, disabled );
    }



    /// <summary>
    /// 令玩家选取一个目标
    /// </summary>
    /// <param name="targetType">目标类型</param>
    /// <param name="token">取消标识</param>
    /// <returns>玩家选择的目标</returns>
    protected virtual async Task<object> CherryTarget( Type targetType, CancellationToken token )
    {

      if ( targetType == null )
        return null;

      else if ( typeof( StandardCardGamePlayer ).IsAssignableFrom( targetType ) )
      {
        var player = await CherryPlayer( token );
        if ( !player.GetType().IsAssignableFrom( targetType ) )
          throw new InvalidOperationException();

        else
          return player;
      }

      else
        throw new NotSupportedException();
    }


    /// <summary>
    /// 选取一个玩家
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    protected virtual Task<StandardCardGamePlayer> CherryPlayer( CancellationToken token )
    {
      throw new NotImplementedException();
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
        return Task.Run( () => { } );
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


      return Task.Run( () => { } );
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
