using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class OverturnCard : SimpleGameCard
  {



    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      AnnounceSpecialCardUsed( user );
      foreach ( var player in user.Game.Players )
      {
        player.Purify();
        player.ClearCards();
        player.HealthPoint -= 1;
        player.PlayerHost.WriteWarningMessage( "您正在遭受伤害导致 HP 减少 1 点，目前 HP 还有 {0} 点", player.HealthPoint );
      }
    }

    public override string Name
    {
      get { return "掀桌"; }
    }

    public override string Description
    {
      get { return "把桌子掀了，不玩了。所有玩家状态和卡牌重置，所有玩家扣除 1 点 HP 。"; }
    }
  }
}
