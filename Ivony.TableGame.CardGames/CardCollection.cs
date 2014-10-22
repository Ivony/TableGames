using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  public class CardCollection : ICardCollection
  {

    public CardCollection()
    {
      CardList = new List<Card>();
      SyncRoot = new object();
    }

    public object SyncRoot { get; private set; }

    protected List<Card> CardList { get; private set; }

    public bool AddCard( Card card )
    {
      lock ( SyncRoot )
      {
        if ( CardList.Contains( card ) )
          return false;

        CardList.Add( card );
        return true;
      }
    }

    public bool RemoveCard( Card card )
    {
      lock ( SyncRoot )
      {
        return CardList.Remove( card );
      }
    }

    public int Count
    {
      get { return CardList.Count; }
    }

    public bool Contains( Card card )
    {
      lock ( SyncRoot )
      {
        return CardList.Contains( card );
      }
    }

    public void Clear()
    {
      lock ( SyncRoot )
      {
        CardList.Clear();
      }
    }

    public IEnumerator<Card> GetEnumerator()
    {
      return CardList.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
