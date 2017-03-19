using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  /// <summary>
  /// 游戏宿主状态
  /// </summary>
  public enum GameHostState
  {
    /// <summary>游戏正在初始化，此时不能加入游戏</summary>
    Initializing,
    /// <summary>游戏已经初始化，正在等待玩家加入游戏</summary>
    WaitForPlayer,
    /// <summary>游戏正在运行，此时不能加入游戏</summary>
    Running,
  }
}