using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class DiscardCard : BasicCard
  {
    public override string Description
    {
      get
      {
        return "丢弃目前手上所有的 攻击 和 盾牌 卡牌，重新摸牌";
      }
    }

    public override string Name
    {
      get
      {
        return "弃牌";
      }
    }

    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {
      user.DiscardCards( item => item is AttackCard || item is ShieldCard );
      return Task.CompletedTask;
    }

    public override int ActionPoint { get { return 0; } }
  }
}
