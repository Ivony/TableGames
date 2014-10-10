using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{

  /// <summary>
  /// 攻击牌
  /// </summary>
  public class AttackCard : SimpleGameCard
  {
    public override string Name
    {
      get { return "攻击卡牌"; }
    }

    public override string Description
    {
      get { return "攻击任何一个玩家"; }
    }

    public override Task Execute( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      throw new NotImplementedException();
    }
  }
}
