using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public abstract class BasicCard<TPlayer> : Card where TPlayer : BasicGamePlayer
  {

    public abstract Task UseCard( TPlayer user, TPlayer target );


    public virtual async Task Attack( int attackPoint, TPlayer targetPlayer )
    {
      targetPlayer.HealthPoint -= attackPoint;
    }


  }
}
