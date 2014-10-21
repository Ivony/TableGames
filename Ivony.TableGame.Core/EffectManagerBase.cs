using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 效果管理器
  /// </summary>
  public class EffectManagerBase
  {

    /// <summary>
    /// 创建 EffectManagerBase 实例
    /// </summary>
    /// <param name="game"></param>
    public EffectManagerBase( GameBase game )
    {
      Game = game;
      SyncRoot = new object();
    }

    public GameBase Game { get; private set; }



    /// <summary>
    /// 当游戏事件发生时
    /// </summary>
    /// <param name="gameEvent">发生的游戏事件</param>
    /// <returns></returns>
    public virtual Task OnGameEvent( IGameEvent gameEvent )
    {

      var behaviorEvent = gameEvent as IGameBehaviorEvent;
      if ( behaviorEvent != null )
        return OnGameEvent( behaviorEvent );


      var playerEvent = gameEvent as IGamePlayerEvent;
      if ( playerEvent != null )
        return OnGameEvent( playerEvent );


      return Task.Run( () => { } );
    }



    protected virtual async Task OnGameEvent( IGamePlayerEvent playerEvent )
    {
      var effects = GetPlayerEffects<IGamePlayerEffect>( playerEvent.Player );
      foreach ( var item in effects )
        await item.OnGamePlayerEvent( playerEvent );
    }

    protected virtual async Task OnGameEvent( IGameBehaviorEvent behaviorEvent )
    {

      var initiatorEffects = GetPlayerEffects<IGameBehaviorEffect>( behaviorEvent.InitiatePlayer );

      foreach ( var item in initiatorEffects )
        await item.OnBehaviorInitiator( behaviorEvent );




      var recipientEffects = GetPlayerEffects<IGameBehaviorEffect>( behaviorEvent.RecipientPlayer );

      foreach ( var item in initiatorEffects )
        await item.OnBehaviorRecipient( behaviorEvent );
    }





    protected virtual IEffect[] GetPlayerEffects( GamePlayerBase player )
    {
      lock ( SyncRoot )
      {
        return playerEffects[player].ToArray();
      }
    }


    /// <summary>
    /// 获取指定玩家附加的指定类型的效果列表
    /// </summary>
    /// <typeparam name="TEffect">效果类型</typeparam>
    /// <param name="player">获取效果的玩家</param>
    /// <returns>效果列表</returns>
    protected virtual TEffect[] GetPlayerEffects<TEffect>( GamePlayerBase player ) where TEffect : IEffect
    {
      return GetPlayerEffects( player ).OfType<TEffect>().ToArray();
    }




    protected object SyncRoot { get; private set; }


    private EffectCollection globalEffects = new EffectCollection();
    private Dictionary<GamePlayerBase, EffectCollection> playerEffects = new Dictionary<GamePlayerBase, EffectCollection>();






    /// <summary>
    /// 尝试添加一个全局效果
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    public virtual bool TryAddEffect( IEffect effect )
    {
      lock ( globalEffects.SyncRoot )
      {
        return globalEffects.TryAdd( effect );
      }
    }


    /// <summary>
    /// 尝试添加一个玩家效果
    /// </summary>
    /// <param name="player"></param>
    /// <param name="effect"></param>
    /// <returns></returns>
    public virtual bool TryAddEffect( GamePlayerBase player, IEffect effect )
    {

      if ( !Game.Players.Contains( player ) )
        return false;


      if ( !CanAddEffect( player, effect ) )
        return false;

      lock ( SyncRoot )
      {
        var collection = GetPlayerEffectsCollection( player );

        RemoveMutex( effect, collection );

        return collection.TryAdd( effect );
      }
    }


    /// <summary>
    /// 获取指定玩家的效果容器
    /// </summary>
    /// <param name="player">要获取效果容器的玩家</param>
    /// <returns>效果容器</returns>
    protected virtual EffectCollection GetPlayerEffectsCollection( GamePlayerBase player )
    {
      EffectCollection collection;
      if ( playerEffects.ContainsKey( player ) )
        collection = playerEffects[player];

      else
        playerEffects.Add( player, collection = new EffectCollection() );
      return collection;
    }



    /// <summary>
    /// 派生类实现此方法，判断效果是否可以附加到玩家
    /// </summary>
    /// <param name="player"></param>
    /// <param name="effect"></param>
    /// <returns></returns>
    protected virtual bool CanAddEffect( GamePlayerBase player, IEffect effect )
    {
      return true;
    }

    protected virtual void RemoveMutex( IEffect addedEffect, EffectCollection effects )
    {
    }

  }
}
