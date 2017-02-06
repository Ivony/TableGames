using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{

  /// <summary>
  /// 定义交换卡牌，与指定玩家交换所有卡牌
  /// </summary>
  public class ExchangeCard : StandardCard, IOtherPlayerTarget
  {

    /// <summary>
    /// 使用卡牌
    /// </summary>
    /// <param name="user">发起交换的玩家</param>
    /// <param name="target">要交换的目标玩家</param>
    /// <param name="token">取消标识</param>
    /// <returns>用于等待的 Task 对象</returns>
    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {
      user.ExchangeCards( target );
      return Task.CompletedTask;

    }


    public override int ActionPoint
    {
      get { return 0; }
    }

    public override string Name
    {
      get { return "换牌"; }
    }

    public override string Description
    {
      get { return "将目前手上所有牌与另一位玩家手交换"; }
    }
  }
}
