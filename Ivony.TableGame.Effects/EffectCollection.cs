using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public class EffectCollection : IEffectCollection
  {

    public EffectCollection()
    {
      SyncRoot = new object();
      Effects = new HashSet<IEffect>();
    }


    protected HashSet<IEffect> Effects { get; private set; }


    public object SyncRoot { get; private set; }


    public bool AddEffect( IEffect effect )
    {

      lock ( SyncRoot )
      {
        if ( !CanAddEffect( effect ) )
          return false;

        if ( Effects.Add( effect ) )
        {
          RemoveMutex( effect );
          return true;
        }

        else
          return false;
      }
    }

    /// <summary>
    /// 派生类实现此方法，判断效果是否可以附加到玩家
    /// </summary>
    /// <param name="player"></param>
    /// <param name="effect"></param>
    /// <returns></returns>
    protected virtual bool CanAddEffect( IEffect effect )
    {
      return true;
    }

    protected virtual void RemoveMutex( IEffect effect )
    {
    }


    public bool RemoveEffect( IEffect effect )
    {
      return Effects.Remove( effect );
    }

    public int Count
    {
      get { return Effects.Count; }
    }

    public bool Contains( IEffect effect )
    {
      return Effects.Contains( effect );
    }

    public void ClearEffect()
    {
      Effects.Clear();
    }

    public IEnumerator<IEffect> GetEnumerator()
    {
      return Effects.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
