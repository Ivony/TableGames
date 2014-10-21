using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
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


  /// <summary>
  /// 一个只能存放特定类型效果的槽位
  /// </summary>
  /// <typeparam name="TEffect">可以存放的效果类型</typeparam>
  public class TypedEffectSlot<TEffect> : IEffectSlot where TEffect : class, IEffect
  {



    public static implicit operator TEffect( TypedEffectSlot<TEffect> slot )
    {
      return slot.Effect;
    }


    public TEffect Effect
    {
      get;
      private set;
    }

    IEffect IEffectSlot.Effect { get { return Effect; } }

    public bool SetEffect( IEffect effect )
    {
      var item = effect as TEffect;
      if ( item != null )
      {
        Effect = item;
        return true;
      }

      else
        return false;
    }


    public bool Contains( IEffect effect )
    {
      return object.Equals( effect, Effect );
    }


    bool IEffectSlot.RemoveEffect( IEffect effect )
    {
      if ( object.Equals( effect, Effect ) )
      {
        RemoveEffects();
        return true;
      }

      else
        return false;
    }


    public void RemoveEffects()
    {
      Effect = null;

    }

    public int Count
    {
      get { return Effect == null ? 0 : 1; }

    }
  }

}
