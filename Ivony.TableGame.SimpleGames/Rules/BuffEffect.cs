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



    protected override Task OnPlayerRoundEvent( PlayerRoundEvent roundEvent )
    {
      var player = (SimpleGamePlayer) roundEvent.Player;
      player.PlayerHost.WriteMessage( $"{Name}，还剩 {rounds} 个回合" );

      if ( rounds-- == 0 )
        player.Effects.RemoveEffect( this );

      return Task.CompletedTask;
    }

  }
}
