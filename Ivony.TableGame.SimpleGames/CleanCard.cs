using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class CleanCard : SimpleGameCard
  {
    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.RemoveAllCard();
      user.Game.AnnounceMessage( "{0} 使用了一张特殊卡牌", user.CodeName );
      user.PlayerHost.WriteMessage( "您手上的卡牌已经清空，请等待下次发牌" );
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
