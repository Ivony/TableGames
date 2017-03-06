using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  public class CardCollection<TCard> : ICardCollection<TCard> where TCard : Card
  {



    protected List<TCard> Collection { get; } = new List<TCard>();

    protected object SyncRoot { get; } = new object();


    public int Count { get { return Collection.Count; } }



    protected void DealCards( ICardDealer<TCard> dealer, int count )
    {
      for ( var i = 0; i < count; i++ )
      {
        AddCard( dealer.DealCard() );
      }
    }


    public bool AddCard( TCard card )
    {
      lock ( SyncRoot )
      {
        Collection.Add( card );
        return true;
      }
    }

    public void Clear()
    {
      lock ( SyncRoot )
      {
        Collection.Clear();
      }

    }

    public bool Contains( TCard card )
    {
      lock ( SyncRoot )
      {
        return Collection.Contains( card );
      }
    }

    public bool RemoveCard( TCard card )
    {
      lock ( SyncRoot )
      {
        return Collection.Remove( card );
      }
    }

    public void RemoveCards( Func<TCard, bool> predicate )
    {
      lock ( SyncRoot )
      {
        foreach ( var card in Collection.ToArray() )
        {
          if ( predicate( card ) )
            Collection.Remove( card );
        }
      }
    }


    public IEnumerator<Card> GetEnumerator()
    {
      return Collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
