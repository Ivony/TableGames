using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class DevilCard : SimpleGameCard, ISelfTarget
  {
    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.SetEffect( new CardEffect() );
      AnnounceSpecialCardUsed( user );
      user.PlayerHost.WriteMessage( "您与恶魔签订了契约，若到您下次发牌之前您没有受到攻击，将获得 HP ，否则攻击将变成双倍" );
    }

    public override string Name
    {
      get { return "恶魔"; }
    }

    public override string Description
    {
      get { return "与恶魔达成契约，若到下次你出牌之前，你未受到攻击，将获得 HP ，但是如果下次轮到你出牌之前受到攻击，将受到双倍伤害。"; }
    }

    public override string ToString()
    {
      return "D";
    }


    private class CardEffect : SimpleGameEffect, IBlessEffect, IAroundEffect
    {
      public override string Name
      {
        get { return "恶魔"; }
      }

      public override string Description
      {
        get { return "恶魔契约生效期间，攻击受到双倍伤害"; }
      }



      protected override async Task OnAttack( AttackEvent attackEvent )
      {
        var player = attackEvent.RecipientPlayer;

        attackEvent.AnnounceAttackEffective();
        player.HealthPoint -= attackEvent.AttackPoint * 2;
        player.PlayerHost.WriteWarningMessage( "您输掉了恶魔契约，受到双倍伤害 {0} 点，目前 HP {1}", attackEvent.AttackPoint * 2, player.HealthPoint );

        player.Effects.RemoveEffect( this );
        attackEvent.Handled = true;
      }


      public override string ToString()
      {
        return "D";
      }

      public async Task OnTurnedAround( SimpleGamePlayer player )
      {
        var point = 10;
        player.Effects.RemoveEffect( this );
        player.HealthPoint += point;
        player.PlayerHost.WriteMessage( "您赢得了恶魔的契约，增加 HP {0} 点", point );
      }
    }

  }
}
