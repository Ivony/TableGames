using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义一个需要玩家选择的选项
  /// </summary>
  public interface IOption
  {

    /// <summary>
    /// 选项名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 选项描述
    /// </summary>
    string Description { get; }

  }
}
