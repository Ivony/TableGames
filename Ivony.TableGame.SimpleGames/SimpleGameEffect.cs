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
      return null;
    }

    async Task IGameBehaviorEffect.OnBehaviorRecipient( IGameBehaviorEvent gameEvent )
    {
      var attackEvent = gameEvent as AttackEvent;
      if ( attackEvent != null )
        await OnAttack( attackEvent );
    }

    protected virtual Task OnAttack( AttackEvent attackEvent )
    {
      return null;
    }


    public abstract string Name { get; }

    public abstract string Description { get; }
  }
}
