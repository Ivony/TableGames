using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Cards
{
  public class AttackEffect : EffectBase
  {

    public AttackEffect( Player player, Cabin target, int points ) : base( player, target ) { AttackPoints = points; }


    public int AttackPoints
    {
      get;
      private set;
    }

    public override void ApplyEffect()
    {

      var targetPlayer = Target.Player;
      if ( targetPlayer != null )
        targetPlayer.WriteMessage( new GenericMessage( GameMessageType.Warning, string.Format( "您受到攻击，生命值损失 {0} 点", AttackPoints ) ) );


      Announce( "{0} 玩家对 {1} 舱发起攻击。", Player.CodeName, Target.Index );


      Target.Damage( AttackPoints );
    }

  }
}
