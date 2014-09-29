using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards.Core.Cards
{
  public class AttackEffect : EffectBase
  {

    public AttackEffect( Player player, Cabin target, int points ) : base( player, target ) { AttackPoints = points; }


    public int AttackPoints
    {
      get;
      private set;
    }

    public override void ApplyEffect()
    {
      Target.Damage( AttackPoints );
    }
  }
}
