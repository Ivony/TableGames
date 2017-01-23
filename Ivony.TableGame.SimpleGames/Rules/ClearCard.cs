using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ClearCard : SimpleGameCard, ISelfTarget
  {
    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      AnnounceSpecialCardUsed( user );
      user.ClearCards();

      return Task.CompletedTask;
    }


    public override string Name
    {
      get { return "清空"; }
    }

    public override string Description
    {
      get { return "清空目前手上所有的卡牌，重新发牌"; }
    }
  }
}
