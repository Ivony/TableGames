using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  internal sealed class NotSupportEffectCollection : IEffectCollection
  {
    bool IEffectCollection.AddEffect( IEffect effect )
    {
      return false;
    }

    bool IEffectCollection.RemoveEffect( IEffect effect )
    {
      return false;
    }

    int IEffectCollection.Count
    {
      get { return 0; }
    }

    bool IEffectCollection.Contains( IEffect effect )
    {
      return false;
    }

    void IEffectCollection.ClearEffect()
    {
    }

    IEnumerator<IEffect> IEnumerable<IEffect>.GetEnumerator()
    {
      return Enumerable.Empty<IEffect>().GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return Enumerable.Empty<IEffect>().GetEnumerator();
    }
  }
}
