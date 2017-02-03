using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ivony.TableGame.CardGames;

namespace Ivony.TableGame.SimpleGames.Rules
{
  /// <summary>
  /// 盗窃卡牌
  /// </summary>
  public class StealCard : BasicCard, IOtherPlayerTarget
  {
    public override string Description
    {
      get { return "从别人手里偷回来一张牌"; }
    }

    public override string Name
    {
      get { return "盗窃"; }
    }

    public override async Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {

      await target.StealCardBy( user, token );

    }

    public override int ActionPoint
    {
      get { return 0; }
    }
  }
}
