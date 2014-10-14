using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class AngelCard : SimpleGameCard
  {


    public override async Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.SpecialEffect = Effects.AngelEffect();
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
  }
}
