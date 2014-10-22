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


    protected virtual async Task<Card> CherryCard( CancellationToken token )
    {

      var card = await PlayerHost.Console.Choose( "请出牌：", Cards, null, token );

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
    /// 获取或设置生命点数（若在回合结束后，某玩家的生命点数小于等于 0 ，该玩家将退出游戏）
    /// </summary>
    public int HealthPoint { get; set; }


    /// <summary>
    /// 获取或设置行动点数（若行动力点数小于目前所要执行的行动点数，则不能行动）
    /// </summary>
    public int ActionPoint { get; set; }



    internal IBasicGame InternalGame { get { return (IBasicGame) GameHost.Game; } }


    /// <summary>
    /// 重写此方法退出游戏
    /// </summary>
    public override void QuitGame()
    {
      InternalGame.OnPlayerQuitted( this );
      base.QuitGame();
    }



    /// <summary>
    /// 当玩家死亡时调用此方法。
    /// </summary>
    public virtual Task Dead()
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
    public Task OnGameEvent( IGameEvent gameEvent )
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
