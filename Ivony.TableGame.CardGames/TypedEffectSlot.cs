using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  /// <summary>
  /// 一个只能存放特定类型效果的槽位
  /// </summary>
  /// <typeparam name="TEffect">可以存放的效果类型</typeparam>
  public class TypedEffectSlot<TEffect> : IEffectSlot where TEffect : class, IEffect
  {



    /// <summary>
    /// 定义效果槽位到效果对象的隐式类型转换
    /// </summary>
    /// <param name="slot">效果槽位</param>
    /// <returns>槽位中的效果</returns>
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
