using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 用于声明程序集中包含游戏
  /// </summary>
  [AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
  public class GameAttribute : Attribute
  {


    /// <summary>
    /// 声明一个游戏
    /// </summary>
    /// <param name="type">用于创造游戏实例的类型</param>
    public GameAttribute( Type type )
    {
      GameFactoryType = type;
    }


    /// <summary>
    /// 用于创造游戏实例的类型
    /// </summary>
    public Type GameFactoryType { get; }



  }
}
