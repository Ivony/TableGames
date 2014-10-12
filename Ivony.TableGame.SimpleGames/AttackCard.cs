using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{

  /// <summary>
  /// 攻击卡牌
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

    public async override Task Execute( SimpleGamePlayer user, SimpleGamePlayer target )
    {


      if ( target.AngelState )
      {
        target.AngelState = false;
        target.Health += Point;
        user.GameHost.Game.AnnounceMessage( "{0} 对 {1} 发起攻击，但攻击无效", user.CodeName, target.CodeName );
        target.PlayerHost.WriteMessage( "天使保护你，攻击变为治疗效果，增加 {0} 点 HP", Point );
        return;
      }

      else if ( target.DevilState )
      {
        target.DevilState = false;
        target.Health -= Point * 2;
        user.GameHost.Game.AnnounceMessage( "{0} 对 {1} 发起攻击。", user.CodeName, target.CodeName );
        target.PlayerHost.WriteWarningMessage( "您输掉了恶魔契约，受到双倍伤害 {0} 点，目前生命值 {1}", Point * 2, target.Health );
        return;
      }

      else if ( target.ShieldState )
      {
        target.ShieldState = false;
        user.GameHost.Game.AnnounceMessage( "{0} 对 {1} 发起攻击，但攻击无效", user.CodeName, target.CodeName );
        target.PlayerHost.WriteMessage( "您使用盾牌阻挡了 {0} 点攻击，防御效果已经失效", Point );
        return;
      }

      else
      {
        target.Health -= Point;
        user.GameHost.Game.AnnounceMessage( "{0} 对 {1} 发起攻击。", user.CodeName, target.CodeName );
        target.PlayerHost.WriteWarningMessage( "您受到攻击，生命值减少 {0} 点，目前生命值 {1}", Point, target.Health );
      }
    }
  }
}
