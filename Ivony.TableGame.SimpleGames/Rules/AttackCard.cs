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
  public class AttackCard : ElementAttachmentCard, IOtherPlayerTarget
  {

    /// <summary>
    /// 创建攻击卡牌对象
    /// </summary>
    /// <param name="point">攻击点数</param>
    public AttackCard( int point = 1 )
    {
      Point = point;
    }


    /// <summary>
    /// 攻击点数
    /// </summary>
    public int Point { get; }

    public override string Name
    {
      get { return string.Format( "攻击", Point ); }
    }

    public override string Description
    {
      get { return string.Format( "攻击任何一个玩家，造成伤害", Point ); }
    }

    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {

      var attackEvent = new AttackEvent( user, target, Element, Point );
      await user.Game.OnGameEvent( attackEvent );

      if ( !attackEvent.Handled )
      {
        target.HealthPoint -= Point;
        user.Game.AnnounceMessage( "{0} 对 {1} 发起{2}攻击。", user.PlayerName, target.PlayerName, Element == null ? "" : Element.Name + "属性" );
        target.PlayerHost.WriteWarningMessage( "您受到攻击，HP 减少 {0} 点，目前 HP {1}", Point, target.HealthPoint );
      }
    }
  }
}
