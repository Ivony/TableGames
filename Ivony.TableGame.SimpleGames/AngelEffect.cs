using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class AngelEffect : SimpleGameEffect
  {
    public override string Name
    {
      get { return "天使"; }
    }

    public override string Description
    {
      get { return "天使将使得对你的攻击转化为治疗"; }
    }


    public override async Task<bool> OnAttack( SimpleGamePlayer user, SimpleGamePlayer target, int point )
    {
      target.SpecialEffect.Clear();
      target.Health += point;
      Game.AnnounceMessage( "{0} 对 {1} 发起攻击，但攻击无效", user.CodeName, target.CodeName );
      target.PlayerHost.WriteMessage( "天使保护你，攻击变为治疗效果，增加 {0} 点 HP", point );
      return false;
    }
  }
}
