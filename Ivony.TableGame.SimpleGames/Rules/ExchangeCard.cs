using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ExchangeCard : BasicCard, ISelfTarget
  {
    public override async Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {

    }


    public override int ActionPoint
    {
      get { return 1; }
    }

    public override string Name
    {
      get { return "换牌"; }
    }

    public override string Description
    {
      get { return "将目前手上的一张牌与其他玩家手上的一张牌进行交换"; }
    }
  }
}
