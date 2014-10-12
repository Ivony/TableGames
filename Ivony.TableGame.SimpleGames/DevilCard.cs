using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class DevilCard : SimpleGameCard
  {
    public async override Task Execute( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.Game.AnnounceMessage( "{0} 使用了一张特殊卡牌", user.CodeName );
      user.PlayerHost.WriteMessage( "您与恶魔签订了契约，若到您下次发牌之前您没有受到攻击，将获得 HP ，否则攻击将变成双倍" );
      user.DevilState = true;
      user.AngelState = false;
    }

    public override string Name
    {
      get { return "恶魔"; }
    }

    public override string Description
    {
      get { return "与恶魔达成契约，若到下次你出牌之前，你未受到攻击，将获得 HP ，但是如果下次轮到你出牌之前受到攻击，将受到双倍伤害。"; }
    }
  }
}
