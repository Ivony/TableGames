using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义一个与某个特定玩家相关的游戏事件
  /// </summary>
  public interface IGamePlayerEvent : IGameEvent
  {
    /// <summary>
    /// 相关的玩家
    /// </summary>
    GamePlayerBase Player { get; }
  }
}
