using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public abstract class BasicCard : Card
  {

    public abstract Task UseCard( BasicGamePlayer user, BasicGamePlayer target );


    public virtual async Task Attack( int attackPoint, BasicGamePlayer targetPlayer )
    {
      targetPlayer.HealthPoint -= attackPoint;
    }


    public virtual async Task ApplyEffect( PlayerEffect effect, BasicGamePlayer targetPlayer )
    {
      targetPlayer.ApplyEffect( effect );
    }


  }
}
