using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class DevilEffect : SimpleGameEffect
  {
    public override string Name
    {
      get { return "恶魔"; }
    }

    public override string Description
    {
      get { return "恶魔契约生效期间，攻击受到双倍伤害"; }
    }

    public override async Task<bool> OnAttack( SimpleGamePlayer user, SimpleGamePlayer target, int point )
    {
      target.SpecialEffect.Clear();
      target.Health -= point * 2;
      target.PlayerHost.WriteWarningMessage( "您输掉了恶魔契约，受到双倍伤害 {0} 点，目前生命值 {1}", point * 2, target.Health );
      return true;
    }
  }
}
