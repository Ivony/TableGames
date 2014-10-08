using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义可持久化的游戏宿主
  /// </summary>
  public interface IPersistableGameHost : IGameHost
  {

    /// <summary>
    /// 保存游戏
    /// </summary>
    /// <param name="name">保存的位置名称</param>
    void Save( string name );

    /// <summary>
    /// 加载游戏
    /// </summary>
    /// <param name="name">读取的位置名称</param>
    void Load( string name );

  }
}
