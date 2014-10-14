using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SpecialEffectSlot : SimpleGamePlayerEffectSlot
  {

    public override bool CanSetEffect( SimpleGameEffect effect )
    {

      return effect is AngelEffect
          || effect is DevilEffect;
    }


    public override string ToString()
    {
      if ( Effect is AngelEffect )
        return "A";

      else if ( Effect is DevilEffect )
        return "D";

      else
        return " ";
    }

  }
}
