using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class DevilEffect : ISpecialEffect
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
      target.PlayerHost.WriteWarningMessage( "您输掉了恶魔契约，受到双倍伤害 {0} 点，目前生命值 {1}", point * 2, target.HealthPoint );
      return true;
    }

    public void Win( SimpleGamePlayer player )
    {
      var point = 10;
      player.SpecialEffect = null;
      player.HealthPoint += point;
      player.PlayerHost.WriteMessage( "您赢得了恶魔的契约，增加 HP {0} 点", point );
    }
  }
}
