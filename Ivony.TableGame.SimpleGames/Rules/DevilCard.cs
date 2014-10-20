﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class DevilCard : SimpleGameCard
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


    private class CardEffect : IBlessEffect, IAroundEffect
    {
      public string Name
      {
        get { return "恶魔"; }
      }

      public string Description
      {
        get { return "恶魔契约生效期间，攻击受到双倍伤害"; }
      }

      public async Task<bool> OnAttack( SimpleGamePlayer user, SimpleGamePlayer target, int point )
      {
        target.SpecialEffect = null;
        target.HealthPoint -= point * 2;
        target.PlayerHost.WriteWarningMessage( "您输掉了恶魔契约，受到双倍伤害 {0} 点，目前 HP {1}", point * 2, target.HealthPoint );
        return true;
      }


      public override string ToString()
      {
        return "D";
      }

      public async Task OnTurnedAround( SimpleGamePlayer player )
      {
        var point = 10;
        player.SpecialEffect = null;
        player.HealthPoint += point;
        player.PlayerHost.WriteMessage( "您赢得了恶魔的契约，增加 HP {0} 点", point );
      }
    }

  }
}
