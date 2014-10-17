using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ReboundCard : SimpleGameCard
  {
    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      AnnounceSpecialCardUsed( user );
      user.DefenceEffect = new CardEffect();
    }

    public override string Name
    {
      get { return "反弹"; }
    }

    public override string Description
    {
      get { return "到下一次发牌之前，您遭受的第一次伤害将反弹给攻击者"; }
    }

    private class CardEffect : IDefenceEffect, IAroundEffect
    {
      public async Task<bool> OnAttack( SimpleGamePlayer user, SimpleGamePlayer target, int point )
      {
        target.DefenceEffect = null;
        user.HealthPoint -= point;
        user.PlayerHost.WriteWarningMessage( "您对 {0} 发起的攻击被反弹了，您的 HP 减少 {1} 点，目前 HP {2} 点", target.PlayerName, point, user.HealthPoint );
        target.PlayerHost.WriteMessage( "您遭受了 {0} 点攻击，伤害已经反弹给攻击者，反弹效果已失效。", point );
        return false;
      }

      public string Name
      {
        get { return "反弹"; }
      }

      public string Description
      {
        get { return "当遭受攻击时，将攻击反弹给攻击者"; }
      }

      public override string ToString()
      {
        return "R";
      }

      public async Task OnTurnedAround( SimpleGamePlayer player )
      {
        player.DefenceEffect = null;
        player.PlayerHost.WriteWarningMessage( "时间到，反弹效果已经自动解除" );
      }
    }

  }
}
