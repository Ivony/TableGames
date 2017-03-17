using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  /// <summary>
  /// 定义一个创建游戏对象的工厂类
  /// </summary>
  public interface IGameFactory
  {

    /// <summary>
    /// 游戏名称
    /// </summary>
    string GameName { get; }

    /// <summary>
    /// 游戏描述
    /// </summary>
    string GameDescription { get; }

    /// <summary>
    /// 创建游戏对象
    /// </summary>
    /// <param name="args">游戏参数</param>
    /// <returns>游戏对象</returns>
    GameBase CreateGame( string[] args );

  }
}
