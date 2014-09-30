using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Cards
{
  public class NormalAttackCard : AttackCard
  {


    public NormalAttackCard( int points )
    {
      Points = points;
    }

    public override string Name
    {
      get { return "普通攻击"; }
    }

    public override string Description
    {
      get { return "对指定位置进行一次普通攻击"; }
    }

    public int Points
    {
      get;
      private set;
    }

    public override EffectBase GetEffect( Player player, Cabin target )
    {
      return new AttackEffect( player, target, Points );
    }
  }
}
