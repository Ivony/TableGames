using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class DefenceEffectSlot : SimpleGamePlayerEffectSlot
  {

    public override bool CanSetEffect( SimpleGameEffect effect )
    {
      return effect is ShieldEffect;
    }

    public override string ToString()
    {
      if ( Effect is ShieldEffect )
        return "S";

      else
        return " ";
    }

  }
}
