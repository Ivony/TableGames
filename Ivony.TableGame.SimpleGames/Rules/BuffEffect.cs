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

    private int? rounds;




    protected override bool Available
    {
      get { return rounds != null; }
    }

    protected override Task OnPlayerRoundEvent( PlayerRoundEvent roundEvent )
    {
      if ( rounds == null )
        rounds = 5;

      if ( rounds < 0 )
        throw new InvalidOperationException();

      var player = (SimpleGamePlayer) roundEvent.Player;
      player.PlayerHost.WriteMessage( (this is CurseEffect ? "诅咒" : "祝福") + $"，还剩 {rounds} 个回合" );

      if ( rounds-- == 0 )
        player.Effects.RemoveEffect( this );

      return Task.CompletedTask;
    }
  }
}
