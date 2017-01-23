using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{


  /// <summary>
  /// 选项提供程序，提供一个方法，可以通过值来创建 Option 对象
  /// </summary>
  public static class OptionProvider
  {


    /// <summary>
    /// 通过指定值创建 Option 对象
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    /// <param name="value">值对象</param>
    /// <returns>选项对象</returns>
    public static Option CreateOption<T>( T value )
    {

      var player = value as GamePlayerBase;
      if ( player != null )
        return new Option( player.PlayerName, "玩家 - " + player.PlayerName );


      return null;

    }
  }
}
