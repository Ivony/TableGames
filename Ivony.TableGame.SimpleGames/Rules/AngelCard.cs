using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class AngelCard : SimpleGameCard
  {


    public override async Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.SpecialEffect = new CardEffect();
      AnnounceSpecialCardUsed( user );

      user.PlayerHost.WriteMessage( "天使保护你，下一次攻击将变成治疗" );
    }

    public override string Name
    {
      get { return "天使"; }
    }

    public override string Description
    {
      get { return "天使卡牌让你下次遭受的攻击成为治疗，加上相应的 HP"; }
    }


    public class CardEffect : ISpecialEffect
    {
      public string Name
      {
        get { return "天使"; }
      }

      public string Description
      {
        get { return "天使将使得对你的攻击转化为治疗"; }
      }


      public async Task<bool> OnAttack( SimpleGamePlayer user, SimpleGamePlayer target, int point )
      {
        target.SpecialEffect = null;
        target.HealthPoint += point;
        target.PlayerHost.WriteMessage( "天使保护你，攻击变为治疗效果，增加 {0} 点 HP", point );
        return false;
      }

      public override string ToString()
      {
        return "A";
      }

    }

  }
}
