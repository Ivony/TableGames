using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ShieldEffect : IDefenceEffect
  {
    public string Name
    {
      get { return "盾牌"; }
    }

    public string Description
    {
      get { return "盾牌效果可以抵御一次攻击"; }
    }


    public async Task<bool> OnAttack( SimpleGamePlayer user, SimpleGamePlayer target, int point )
    {
      target.PlayerHost.WriteMessage( "您使用盾牌阻挡了 {0} 点攻击，防御效果已经失效", point );
      return false;
    }

    public override string ToString()
    {
      return "S";
    }

  }
}
