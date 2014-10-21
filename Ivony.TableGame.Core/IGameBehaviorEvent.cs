using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义游戏中出现的玩家行为事件
  /// </summary>
  public interface IGameBehaviorEvent : IGameEvent
  {

    /// <summary>
    /// 行为的发起人
    /// </summary>
    GamePlayerBase InitiatePlayer { get; }

    /// <summary>
    /// 行为的接受者
    /// </summary>
    GamePlayerBase RecipientPlayer { get; }
  }
}
