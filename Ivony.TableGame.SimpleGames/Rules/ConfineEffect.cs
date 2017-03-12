using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivony.TableGame.CardGames;

namespace Ivony.TableGame.SimpleGames.Rules
{
  /// <summary>
  /// 禁锢效果
  /// </summary>
  public class ConfineEffect : SimpleGameEffect, INagativeEffect
  {
    public override string Description
    {
      get { return "被禁锢的玩家一个回合内不得进行任何操作"; }
    }

    public override string Name
    {
      get { return "禁锢"; }
    }

    public override string ToString()
    {
      return "禁";
    }


    protected override Task OnPlayerRoundEvent( PlayerRoundEvent roundEvent )
    {

      var player = (SimpleGamePlayer) roundEvent.Player;
      roundEvent.DataBag.Confine = true;
      player.Effects.RemoveEffect( this );


      return base.OnPlayerRoundEvent( roundEvent );
    }
  }
}
