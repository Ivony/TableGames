using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 定义标准卡牌实现
  /// </summary>
  public abstract class StandardCard : Card
  {

    /// <summary>
    /// 使用卡牌所需的移动点数
    /// </summary>
    public virtual int ActionPoint { get { return 0; } }


    public virtual Type TargetType { get { return null; } }


    /// <summary>
    /// 使用一张卡牌
    /// </summary>
    /// <param name="initiatePlayer">发起玩家</param>
    /// <param name="target">目标对象</param>
    /// <returns>获取用于等待处理卡牌使用过程的 Task</returns>
    public abstract Task Play( CardGamePlayer initiatePlayer, object target );

    /// <summary>
    /// 合并多张卡牌
    /// </summary>
    /// <param name="cards">要与当前卡牌合并的卡牌列表</param>
    /// <returns></returns>
    public virtual StandardCard Combine( params Card[] cards )
    {
      throw new NotSupportedException();
    }

  }
}
