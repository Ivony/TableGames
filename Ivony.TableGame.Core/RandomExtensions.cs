using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义一些随机函数的扩展方法
  /// </summary>
  public static class RandomExtensions
  {

    private static readonly Random random = new Random( DateTime.Now.Millisecond );

    /// <summary>
    /// 从数组中随机选择一项
    /// </summary>
    /// <typeparam name="T">数组元素类型</typeparam>
    /// <param name="array">元素数组</param>
    /// <returns>随机选取的一项</returns>
    public static T RandomItem<T>( this T[] array )
    {
      return array[random.Next( array.Length )];
    }

  }
}
