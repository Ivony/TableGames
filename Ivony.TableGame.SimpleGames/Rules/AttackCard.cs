using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{

  /// <summary>
  /// 定义攻击卡牌
  /// </summary>
  public class AttackCard : SimpleGameCard
  {

    public AttackCard( int point = 1 )
    {

      Point = point;

    }


    public int Point { get; private set; }

    public override string Name
    {
      get { return string.Format( "攻击{0}", Point ); }
    }

    public override string Description
    {
      get { return string.Format( "攻击任何一个玩家，造成 {0} 点伤害", Point ); }
    }

    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {

      var gameEvent = new AttackEvent( user, target, Point );


      ISimpleGameEffect effect = (ISimpleGameEffect) target.SpecialEffect ?? target.DefenceEffect;
      if ( effect != null )
      {
        if ( await effect.OnAttack( user, target, Point ) )
          user.Game.AnnounceMessage( "{0} 对 {1} 发起攻击。", user.PlayerName, target.PlayerName );

        else
          user.Game.AnnounceMessage( "{0} 对 {1} 发起攻击，但攻击无效", user.PlayerName, target.PlayerName );

        return;
      }
      else
      {
        target.HealthPoint -= Point;
        user.Game.AnnounceMessage( "{0} 对 {1} 发起攻击。", user.PlayerName, target.PlayerName );
        target.PlayerHost.WriteWarningMessage( "您受到攻击，HP 减少 {0} 点，目前 HP {1}", Point, target.HealthPoint );
      }
    }
  }
}
