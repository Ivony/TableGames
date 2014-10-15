using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ReboundCard : SimpleGameCard
  {
    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.DefenceEffect = Effects.ReboundEffect();
    }

    public override string Name
    {
      get { return "反弹"; }
    }

    public override string Description
    {
      get { return "在生效期间，遭受的伤害将反弹给攻击者"; }
    }
  }
}
