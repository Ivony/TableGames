using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义一个效果槽位
  /// </summary>
  public interface IEffectSlot
  {
    /// <summary>
    /// 槽位中储存的效果
    /// </summary>
    IEffect Effect { get; }

    /// <summary>
    /// 设置效果
    /// </summary>
    /// <param name="effect">要设置的效果</param>
    /// <returns>是否设置成功</returns>
    bool SetEffect( IEffect effect );


    /// <summary>
    /// 确定槽位中是否存在指定的效果
    /// </summary>
    /// <param name="effect">要检测是否存在的效果</param>
    /// <returns>是否存在</returns>
    bool Contains( IEffect effect );

    /// <summary>
    /// 槽位中的效果总数（一般返回1）
    /// </summary>
    int Count { get; }

    /// <summary>
    /// 从槽位中移除指定的效果
    /// </summary>
    /// <param name="effect">要移除的效果</param>
    /// <returns>是否成功</returns>
    bool RemoveEffect( IEffect effect );

    /// <summary>
    /// 从槽位中移除所有的效果
    /// </summary>
    void RemoveEffects();

  }

}
