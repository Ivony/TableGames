using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  /// <summary>
  /// 定义游戏状态枚举
  /// </summary>
  public enum GameState
  {

    /// <summary>游戏尚未初始化，此时不能加入游戏</summary>
    NotInitialized,
    /// <summary>游戏尚未开始</summary>
    NotStarted,
    /// <summary>游戏正在运行</summary>
    Running,
    /// <summary>游戏已经结束</summary>
    End
  }
}
