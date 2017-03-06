using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  /// <summary>
  /// 反向卡
  /// </summary>
  public class ReflexiveCard : StandardCard
  {
    public override string Description
    {
      get
      {
        return "将目前手上的攻击牌全部换成盾牌，盾牌全部换成攻击";
      }
    }

    public override string Name
    {
      get
      {
        return "矛盾";
      }
    }

    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {
      user.ReflexiveCards();
      return Task.CompletedTask;
    }

    public override int ActionPoint { get { return 0; } }
  }
}
