using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public abstract class BuffEffect : SimpleGameEffect
  {

    private int rounds = 5;



    protected virtual Task OnPlayerRoundEvent( PlayerRoundEvent roundEvent )
    {

      if ( rounds-- == 0 )
        ((SimpleGamePlayer) roundEvent.Player).Effects.RemoveEffect( this );

      return Task.CompletedTask;
    }

  }
}
