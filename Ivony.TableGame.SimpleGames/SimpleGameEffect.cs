using Ivony.TableGame;
using Ivony.TableGame.SimpleGames.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public abstract class SimpleGameEffect : IGameBehaviorEffect
  {
    Task IGameBehaviorEffect.OnBehaviorInitiator( IGameBehaviorEvent gameEvent )
    {
      return Task.Run( () => { } );
    }

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
    /// 效果名称
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// 效果描述
    /// </summary>
    public abstract string Description { get; }
  }
}
