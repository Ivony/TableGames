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
    /// <summary>游戏正在初始化，即将切换到 Initialized 状态</summary>
    Initializing,
    /// <summary>游戏已经初始化，正在等待玩家加入游戏</summary>
    Initialized,
    /// <summary>游戏正在启动，将很快切换到 Running 状态</summary>
    Starting,
    /// <summary>游戏正在运行</summary>
    Running,
    /// <summary>游戏正在保存</summary>
    Saving,
    /// <summary>游戏正在加载</summary>
    Loading,
    /// <summary>游戏被挂起，可能是因为有玩家中途退出导致游戏临时挂起</summary>
    Suspend,
    /// <summary>游戏已经结束</summary>
    End
  }
}
