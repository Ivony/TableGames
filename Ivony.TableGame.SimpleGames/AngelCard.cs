using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class AngelCard : SimpleGameCard
  {
    public override async Task Execute( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.Game.AnnounceMessage( "{0} 使用了一张特殊卡牌", user.CodeName );
      user.PlayerHost.WriteMessage( "天使保护你，下一回合的攻击将变成治疗" );
      user.AngelState = true;
      user.DevilState = false;
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
