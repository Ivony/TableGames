using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards.Core.Cards
{
  public class NormalAttackCard : AttackCard, IEffectProvider
  {
    public override string Name
    {
      get { return "普通攻击"; }
    }

    public override string Description
    {
      get { return "对指定位置进行一次普通攻击"; }
    }

    public EffectBase GetEffect( Player player, Cabin target )
    {
      return new AttackEffect( player, target, 1 );
    }
  }
}
