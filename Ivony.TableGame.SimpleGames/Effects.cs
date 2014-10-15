using Ivony.TableGame.SimpleGames.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public static class Effects
  {

    public static ShieldEffect ShieldEffect()
    {
      return new ShieldEffect();
    }


    public static AngelEffect AngelEffect()
    {
      return new AngelEffect();
    }

    public static DevilEffect DevilEffect()
    {
      return new DevilEffect();
    }

    public static IDefenceEffect ReboundEffect()
    {
      return new ReboundEffect();
    }
  }
}
