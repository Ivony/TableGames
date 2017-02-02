using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义游戏中出现的事件抽象
  /// </summary>
  public interface IGameEvent
  {

    /// <summary>
    /// 事件数据容器
    /// </summary>
    IDictionary<string, object> Data { get; }

  }



  /// <summary>
  /// 代表一个可以并行广播的游戏事件
  /// </summary>
  public interface IParallelGameEvent : IGameEvent
  {

  }


  /// <summary>
  /// 代表一个必须被处理的游戏事件
  /// </summary>
  public interface IGameNeedHandledEvent
  {
    /// <summary>
    /// 事件是否已经被处理
    /// </summary>
    bool Handled { get; }

    /// <summary>
    /// 如果所有人都没有处理这个事件，那么调用这个方法处理事件
    /// </summary>
    /// <returns>用于等待事件处理完毕的 Task 对象</returns>
    Task HandleEvent();
  }

}
