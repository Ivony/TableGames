using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{


  /// <summary>
  /// 基于固定效果槽位的效果容器
  /// </summary>
  public abstract class EffectSlotCollection : IEffectCollection
  {

    protected List<IEffectSlot> Slots { get; private set; }


    public object SyncRoot { get; private set; }


    protected EffectSlotCollection()
    {
      SyncRoot = new object();
      Slots = new List<IEffectSlot>();
    }


    protected void RegisterSlot( IEffectSlot slot )
    {
      lock ( SyncRoot )
      {
        Slots.Add( slot );
      }
    }

    protected void RegisterSlot( params IEffectSlot[] slots )
    {
      lock ( SyncRoot )
      {
        Slots.AddRange( slots );
      }
    }



    public bool AddEffect( IEffect effect )
    {
      lock ( SyncRoot )
      {
        return Slots.Any( item => item.SetEffect( effect ) );
      }
    }

    public bool RemoveEffect( IEffect effect )
    {
      lock ( SyncRoot )
      {
        return Slots.Any( item => item.RemoveEffect( effect ) );
      }
    }

    public int Count
    {
      get
      {
        lock ( SyncRoot )
        {
          return Slots.Sum( item => item.Count );
        }
      }
    }

    public bool Contains( IEffect effect )
    {
      lock ( SyncRoot )
      {
        return Slots.Any( item => item.Contains( effect ) );
      }
    }

    public void ClearEffect()
    {
      lock ( SyncRoot )
      {
        foreach ( var item in Slots )
          item.RemoveEffects();
      }
    }

    public IEnumerator<IEffect> GetEnumerator()
    {
      return Slots.Select( item => item.Effect ).Where( item => item != null ).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    public override string ToString()
    {
      return string.Join( " ", this );
    }

  }
}
