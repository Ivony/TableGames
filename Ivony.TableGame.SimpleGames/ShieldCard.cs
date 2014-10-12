using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class ShieldCard : SimpleGameCard
  {
    public async override Task Execute( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.Shield = true;
      user.GameHost.Game.AnnounceMessage( "{0} 使用了一张特殊卡牌", user.CodeName );
      user.PlayerHost.WriteMessage( "下一次攻击将对您无效。" );

    }

    public override string Name
    {
      get { return "盾牌"; }
    }

    public override string Description
    {
      get { return "使用此卡牌后可以抵挡一次攻击"; }
    }
  }
}
