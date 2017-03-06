using System;
using System.Linq;
using System.Threading.Tasks;
using Ivony.TableGame.CardGames;

namespace Ivony.TableGame.SimpleGames.Rules
{

  /// <summary>
  /// 诅咒效果
  /// </summary>
  internal class CurseEffect : BuffEffect
  {
    /// <summary>
    /// 元素
    /// </summary>
    public Element Element { get; }

    public CurseEffect( Element element )
    {
      Element = element;
    }

    public override string Description
    {
      get
      {
        if ( Element == Element.金 )
          return "金之诅咒，被诅咒者五回合内盾牌有50%概率无效";
        else if ( Element == Element.木 )
          return "木之诅咒，被诅咒者五回合内每回合扣 1 点 HP";
        else if ( Element == Element.水 )
          return "水之诅咒，被诅咒者五回合内若无盾牌抵挡，受到攻击时有50%的概率受到双倍伤害";
        else if ( Element == Element.火 )
          return "火之诅咒，被诅咒者五回合内有50%概率攻击无效，且所有攻击最多能造成 1 点伤害。";
        else if ( Element == Element.土 )
          return "土之诅咒，被诅咒者五回合内将有一次出牌无效。";

        throw new InvalidOperationException();
      }
    }

    public override string Name
    {
      get { return Element.Name + "之诅咒"; }
    }



    protected override Task OnLaunchAttack( AttackEvent attackEvent )
    {
      if ( Element == Element.火 )
      {
        if ( random.Next( 1 ) == 0 )
        {
          attackEvent.InitiatePlayer.PlayerHost.WriteWarningMessage( "你做好了发动攻击的一切准备，就是忘记了带上武器，这一次，你只好无功而返了。" );
          attackEvent.Handled = true;
        }
      }

      return base.OnLaunchAttack( attackEvent );
    }

    protected override Task OnAttacked( AttackEvent attackEvent )
    {

      if ( Element == Element.金 )
      {
        if ( random.Next( 2 ) == 0 )
        {
          attackEvent.RecipientPlayer.PlayerHost.WriteWarningMessage( "当你正打算用盾牌格挡这一次攻击的时候，却发现盾牌无论如何都找不到了" );
          attackEvent.Data["ShieldDisabled"] = true;
        }
      }
      else if ( Element == Element.水 )
      {
        if ( random.Next( 2 ) == 0 )
        {
          attackEvent.Data["DoubleAttack"] = true;
        }
      }

      return base.OnAttacked( attackEvent );
    }



    protected override Task OnPlayerRoundEvent( PlayerRoundEvent roundEvent )
    {
      var player = (SimpleGamePlayer) roundEvent.Player;
      if ( Element == Element.木 )
      {
        player.HealthPoint--;
        player.PlayerHost.WriteWarningMessage( $"你身中奇毒未解，HP - 1，当前 HP: {player.HealthPoint}" );
      }
      else if ( Element == Element.土 )
      {
        if ( random.Next( 10 ) < 3 )
        {
          roundEvent.Data["PlayInvalid"] = true;
        }
      }

      return base.OnPlayerRoundEvent( roundEvent );
    }


    public override string ToString()
    {
      return "咒";
    }
  }
}