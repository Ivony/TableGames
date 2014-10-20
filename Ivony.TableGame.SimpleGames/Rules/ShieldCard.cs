using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ShieldCard : SimpleGameCard
  {
    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.SetEffect( new CardEffect() );
      AnnounceSpecialCardUsed( user );
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


    private class CardEffect : SimpleGameEffect, IDefenceEffect
    {
      public override string Name
      {
        get { return "盾牌"; }
      }

      public override string Description
      {
        get { return "盾牌效果可以抵御一次攻击"; }
      }


      protected override async Task OnAttack( AttackEvent attackEvent )
      {
        attackEvent.AnnounceAttackIneffective();
        attackEvent.RecipientPlayer.PlayerHost.WriteWarningMessage( "您使用盾牌阻挡了 {0} 点攻击，防御效果已经失效", attackEvent.AttackPoint, attackEvent.RecipientPlayer.HealthPoint );
        attackEvent.Handled = true;
      }

      public override string ToString()
      {
        return "S";
      }

    }

  }
}
