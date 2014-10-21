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

  }
}
