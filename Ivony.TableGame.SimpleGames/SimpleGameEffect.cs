using Ivony.TableGame;
using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public abstract class SimpleGameEffect : IGameBehaviorEffect, IGamePlayerEffect
  {


    protected static readonly Random random = new Random( DateTime.Now.Millisecond );


    /// <summary>
    /// 派生类重写此方法当玩家作为事件的发起者要执行的操作
    /// </summary>
    /// <param name="gameEvent">引发的游戏事件</param>
    /// <returns>用于等待事件处理完毕的 Task 对象</returns>
    public virtual async Task OnBehaviorInitiator( IGameBehaviorEvent gameEvent )
    {
      if ( Available == false )
        return;

      var attackEvent = gameEvent as AttackEvent;
      if ( attackEvent != null && attackEvent.Handled == false )
        await OnLaunchAttack( attackEvent );
    }


    /// <summary>
    /// 派生类重写此方法当玩家作为事件的接受者要执行的操作
    /// </summary>
    /// <param name="gameEvent">引发的游戏事件</param>
    /// <returns>用于等待事件处理完毕的 Task 对象</returns>
    public virtual async Task OnBehaviorRecipient( IGameBehaviorEvent gameEvent )
    {
      if ( Available == false )
        return;

      var attackEvent = gameEvent as AttackEvent;
      if ( attackEvent != null && attackEvent.Handled == false )
        await OnAttacked( attackEvent );
    }


    /// <summary>
    /// 派生类重写此方法当发动攻击时要执行的操作
    /// </summary>
    /// <param name="attackEvent">攻击事件</param>
    /// <returns>用于等待事件处理完毕的 Task 对象</returns>
    protected virtual Task OnLaunchAttack( AttackEvent attackEvent )
    {
      return Task.CompletedTask;
    }



    /// <summary>
    /// 派生类重写此方法当被攻击时要执行的操作
    /// </summary>
    /// <param name="attackEvent">攻击事件</param>
    /// <returns>用于等待事件处理完毕的 Task 对象</returns>
    protected virtual Task OnAttacked( AttackEvent attackEvent )
    {
      return Task.CompletedTask;
    }


    /// <summary>
    /// 派生类重写此方法处理玩家事件
    /// </summary>
    /// <param name="gameEvent">玩家事件</param>
    /// <returns>用于等待事件处理完成的 Task 对象</returns>
    public virtual Task OnGamePlayerEvent( IGamePlayerEvent gameEvent )
    {
      if ( Available == false )
        return Task.CompletedTask;

      var round = gameEvent as PlayerRoundEvent;
      if ( round != null )
        return OnPlayerRoundEvent( round );

      else
        return Task.CompletedTask;
    }


    protected virtual Task OnPlayerRoundEvent( PlayerRoundEvent roundEvent )
    {
      return Task.CompletedTask;
    }




    /// <summary>
    /// 效果名称
    /// </summary>
    public abstract string Name { get; }


    /// <summary>
    /// 效果描述
    /// </summary>
    public abstract string Description { get; }



    /// <summary>
    /// 效果在当前是否可用
    /// </summary>
    protected virtual bool Available { get { return true; } }

  }
}
