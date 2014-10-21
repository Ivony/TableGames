using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.BasicCardGames
{


  /// <summary>
  /// 基于效果插槽的效果容器
  /// </summary>
  public abstract class EffectSlotCollection : IEffectCollection
  {

    private List<IEffectSlot> Slots { get; private set; }


    private object _sync = new object();
    public object SyncRoot { get { return _sync; } }


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

    public void ClearEffect( IEffect effect )
    {
      lock ( SyncRoot )
      {
        foreach ( var item in Slots )
          item.RemoveEffects();
      }
    }

    public IEnumerator<IEffect> GetEnumerator()
    {
      return Slots.Select( item => item.Effect ).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
