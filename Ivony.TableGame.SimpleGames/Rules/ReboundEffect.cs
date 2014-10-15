using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ReboundEffect : IDefenceEffect
  {
    public async Task<bool> OnAttack( SimpleGamePlayer user, SimpleGamePlayer target, int point )
    {
      target.DefenceEffect = null;
      user.HealthPoint -= point;
      target.PlayerHost.WriteMessage( "您遭受了 {0} 点攻击，伤害已经反弹给攻击者，反弹效果已失效。", point );
      return false;
    }

    public string Name
    {
      get { return "反弹"; }
    }

    public string Description
    {
      get { return "当遭受攻击时，将攻击反弹给攻击者"; }
    }
  }
}
