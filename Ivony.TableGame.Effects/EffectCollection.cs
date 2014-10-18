using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Effects
{
  public class EffectCollection : ICollection<IEffect>
  {


    public EffectCollection()
    {
      Effects = new HashSet<IEffect>();
      SyncRoot = new object();
    }



    protected HashSet<IEffect> Effects { get; private set; }


    public object SyncRoot { get; private set; }



    void ICollection<IEffect>.Add( IEffect item )
    {

      if ( !TryAdd( item ) )
        throw new InvalidOperationException( "已经存在这个效果" );
    }

    public void Clear()
    {
      lock ( SyncRoot )
      {
        Effects.Clear();
      }
    }

    public bool Contains( IEffect item )
    {
      lock ( SyncRoot )
      {
        return Effects.Contains( item );
      }
    }

    public void CopyTo( IEffect[] array, int arrayIndex )
    {
      lock ( SyncRoot )
      {
        Effects.CopyTo( array, arrayIndex );
      }
    }

    public int Count
    {
      get { return Effects.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public bool Remove( IEffect item )
    {
      lock ( SyncRoot )
      {
        return Effects.Remove( item );
      }
    }

    public IEnumerator<IEffect> GetEnumerator()
    {
      return Effects.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public virtual bool TryAdd( IEffect effect )
    {
      return Effects.Add( effect );
    }
  }
}
