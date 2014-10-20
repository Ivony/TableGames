using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public interface IGameEvent
  {

    /// <summary>
    /// 事件数据容器
    /// </summary>
    IDictionary<string, object> Data { get; }


    /// <summary>
    /// 指示事件是否已经被处理完毕
    /// </summary>
    bool Handled { get; }

  }
}
