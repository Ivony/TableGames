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
    Task IGameBehaviorEffect.OnBehaviorInitiator( IGameBehaviorEvent gameEvent )
    {
      return Task.Run( () => { } );
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameEvent"></param>
    /// <returns></returns>
    async Task IGameBehaviorEffect.OnBehaviorRecipient( IGameBehaviorEvent gameEvent )
    {
      var attackEvent = gameEvent as AttackEvent;
      if ( attackEvent != null && attackEvent.Handled == false )
        await OnAttack( attackEvent );
    }

    protected virtual Task OnAttack( AttackEvent attackEvent )
    {
      return Task.Run( () => { } );
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameEvent"></param>
    /// <returns></returns>
    public Task OnGamePlayerEvent( IGamePlayerEvent gameEvent )
    {
      if ( gameEvent is GameAroundEvent )
      {

        var effect = this as IAroundEffect;
        if ( effect != null )
          effect.OnTurnedAround( (SimpleGamePlayer) gameEvent.Player );

      }

      return Task.Run( () => { } ); 
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
