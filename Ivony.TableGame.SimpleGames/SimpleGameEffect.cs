using Ivony.TableGame.Effects;
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
    public Task OnBehaviorInitiator( IGameBehaviorEvent gameEvent )
    {
      return Task.Run( () => { } );
    }

    public async Task OnBehaviorRecipient( IGameBehaviorEvent gameEvent )
    {
      var attackEvent = gameEvent as AttackEvent;
      if ( attackEvent != null )
        await OnAttack( attackEvent );
    }

    protected virtual Task OnAttack( AttackEvent attackEvent )
    {
      return Task.Run( () => { } );
    }


    public abstract string Name { get; }

    public abstract string Description { get; }
  }
}
