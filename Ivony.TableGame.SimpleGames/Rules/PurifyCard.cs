using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  /// <summary>
  /// 净化卡
  /// </summary>
  public class PurifyCard : BasicCard, IAnyPlayerTarget
  {

    /// <summary>
    /// 使用这张卡牌
    /// </summary>
    /// <param name="user">发起玩家</param>
    /// <param name="target">目标玩家</param>
    /// <returns>获取用于等待使用结束的 Task 对象</returns>
    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {
      AnnounceSpecialCardUsed( user );
      target.Purify();

      return Task.CompletedTask;
    }

    /// <summary>
    /// 卡牌名称
    /// </summary>
    public override string Name
    {
      get { return "净化"; }
    }

    /// <summary>
    /// 卡牌描述
    /// </summary>
    public override string Description
    {
      get { return "清除当前所有玩家所有状态"; }
    }
  }
}
