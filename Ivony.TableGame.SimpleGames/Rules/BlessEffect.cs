using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivony.TableGame.CardGames;

namespace Ivony.TableGame.SimpleGames.Rules
{

  /// <summary>
  /// 定义祝福效果
  /// </summary>
  public class BlessEffect : BuffEffect, IPositiveEffect
  {

    /// <summary>
    /// 元素
    /// </summary>
    public Element Element { get; }


    public BlessEffect( Element element )
    {
      Element = element;
    }



    public override string Description
    {
      get
      {
        if ( Element == Element.金 )
          return "金之祝福，使用者五回合无盾牌承受攻击时，有 50% 概率攻击无效";
        else if ( Element == Element.木 )
          return "木之祝福，使用者五回合内每回合加 1 HP";
        else if ( Element == Element.水 )
          return "水之祝福，使用者五回合内诅咒反弹给施咒者。";
        else if ( Element == Element.火 )
          return "火之祝福，使用者五回合内攻击未被盾牌阻挡时，有 50% 概率双倍伤害。";
        else if ( Element == Element.土 )
          return "土之祝福，使用者五回合有 50% 的机会可以行动两次。";

        throw new InvalidOperationException();
      }
    }

    public override string Name
    {
      get
      {
        return Element.Name + "之祝福";
      }
    }

    protected override Task OnAttacked( AttackEvent attackEvent )
    {
      if ( Element == Element.金 )
      {
        if ( random.Next( 2 ) == 0 )
          attackEvent.DataBag.Ineffective = true;
      }

      return base.OnAttacked( attackEvent );
    }

    protected override Task OnLaunchAttack( AttackEvent attackEvent )
    {
      if ( Element == Element.火 && random.Next( 2 ) == 0 )
        attackEvent.DataBag.DoubleAttack = true;
      return base.OnLaunchAttack( attackEvent );
    }

    protected override Task OnPlayerRoundEvent( PlayerRoundEvent roundEvent )
    {
      var player = (SimpleGamePlayer) roundEvent.Player;

      if ( Element == Element.木 )
      {
        player.HealthPoint++;
        player.PlayerHost.WriteWarningMessage( $"你感到前所未有的精力充沛，HP + 1，当前 HP: {player.HealthPoint}" );
      }
      else if ( Element == Element.土 )
      {
        if ( random.Next( 2 ) == 0 )
          player.ActionPoint = 2;
      }



      return base.OnPlayerRoundEvent( roundEvent );
    }
  }
}
