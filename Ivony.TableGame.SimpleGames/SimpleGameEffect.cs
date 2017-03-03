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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameEvent"></param>
    /// <returns></returns>
    public virtual Task OnBehaviorInitiator( IGameBehaviorEvent gameEvent )
    {
      return Task.CompletedTask;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameEvent"></param>
    /// <returns></returns>
    public virtual async Task OnBehaviorRecipient( IGameBehaviorEvent gameEvent )
    {
      var attackEvent = gameEvent as AttackEvent;
      if ( attackEvent != null && attackEvent.Handled == false )
        await OnAttack( attackEvent );
    }

    protected virtual Task OnAttack( AttackEvent attackEvent )
    {
      return Task.CompletedTask;
    }


    /// <summary>
    /// 派生类重写此方法处理玩家事件
    /// </summary>
    /// <param name="gameEvent">玩家事件</param>
    /// <returns>用于等待事件处理完成的 Task 对象</returns>
    public Task OnGamePlayerEvent( IGamePlayerEvent gameEvent )
    {
      if ( gameEvent is GameAroundEvent )
      {

        var effect = this as IAroundEffect;
        if ( effect != null )
          effect.OnTurnedAround( (SimpleGamePlayer) gameEvent.Player );

      }

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
  }
}
