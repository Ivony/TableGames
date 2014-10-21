using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 
  /// </summary>
  public interface IEffectSlot
  {
    IEffect Effect { get; }

    bool SetEffect( IEffect effect );

    bool Contains( IEffect effect );

    int Count { get; }

    bool RemoveEffect( IEffect effect );

    void RemoveEffects();

  }

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
