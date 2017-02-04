using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
      get { return Element?.Name + "攻击"; }
    }

    public override string Description
    {
      get { return string.Format( "攻击任何一个玩家，造成伤害", Point ); }
    }

    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {
      var attackEvent = new AttackEvent( user, target, Element, Point );
      await user.Game.SendGameEvent( attackEvent );
    }


    public override bool Availables( SimpleGamePlayer player )
    {
      return base.Availables( player ) && player.Game.Rounds > 1;
    }
  }
}
