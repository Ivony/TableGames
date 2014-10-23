using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义游戏事件监听器
  /// </summary>
  public interface IGameEventListener
  {

    /// <summary>
    /// 通过实现此方法监听游戏中发生的事件
    /// </summary>
    /// <param name="gameEvent">发生的游戏事件</param>
    /// <returns>返回一个 Task ，用于等待事件处理完毕</returns>
    Task OnGameEvent( IGameEvent gameEvent );

  }
}
