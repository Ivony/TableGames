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
      get { return string.Format( "攻击 {0}", Point ); }
    }

    public override string Description
    {
      get { return string.Format( "攻击任何一个玩家，造成 {0} 点伤害", Point ); }
    }

    public async override Task Execute( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      target.Health -= Point;
      user.GameHost.Game.AnnounceMessage( "{0} 对 {1} 发起攻击，{1} 生命值减少 {2} 点，目前还有 {3} 点", user.CodeName, target.CodeName, Point, target.Health );
      target.PlayerHost.WriteWarningMessage( "您受到攻击，生命值减少 {0} 点，目前生命值 {1}", Point, target.Health );
    }
  }
}
