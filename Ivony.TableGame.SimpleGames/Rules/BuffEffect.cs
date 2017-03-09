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
      if ( this is CurseEffect )
        player.PlayerHost.WriteWarningMessage( "你感到非常的虚弱，你吃了点儿药似乎好一点了，但是恐怕还会持续 {rounds} 个回合" );
      else if ( this is BlessEffect)
        player.PlayerHost.WriteWarningMessage( "你感到精力充沛，活力无限，不过好景不长，这一状态大概还会持续 {rounds} 个回合" );


      if ( rounds-- == 0 )
        player.Effects.RemoveEffect( this );

      return Task.CompletedTask;
    }
  }
}
