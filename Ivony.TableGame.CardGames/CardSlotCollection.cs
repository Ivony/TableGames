using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  public class CardSlotCollection : ICardCollection
  {

    protected List<ICardSlot> Slots { get; private set; }

    protected CardSlotCollection()
    {
      SyncRoot = new object();
      Slots = new List<ICardSlot>();
    }

    public object SyncRoot { get; private set; }

    public bool AddCard( Card card )
    {
      if ( card == null )
        throw new ArgumentNullException( "card" );

      lock ( SyncRoot )
      {
        var emptySlots = Slots.Where( item => item.Card == null ).ToArray();
        return emptySlots.Any( item => item.SetCard( card ) );
      }
    }

    public bool RemoveCard( Card card )
    {
      if ( card == null )
        throw new ArgumentNullException( "card" );

      lock ( SyncRoot )
      {
        return Slots.Any( item => card.Equals( item.Card ) && item.RemoveCard() );
      }
    }

    public int Count
    {
      get
      {
        lock ( SyncRoot )
        {
          return Slots.Count( item => item.Card != null );
        }
      }
    }

    public bool Contains( Card card )
    {
      if ( card == null )
        throw new ArgumentNullException( "card" );

      lock ( SyncRoot )
      {
        return Slots.Any( item => card.Equals( item.Card ) );
      }
    }

    public void Clear()
    {
      lock ( SyncRoot )
      {
        foreach ( var item in Slots.Where( item => item.HasCard ) )
          item.RemoveCard();
      }
    }

    public IEnumerator<Card> GetEnumerator()
    {
      return Slots.Where( item => item.Card != null ).Select( item => item.Card ).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }


    public void DealCards()
    {
      lock ( SyncRoot )
      {
        foreach ( var item in Slots.Where( item => item.HasCard == false ) )
          item.DealCard();
      }
    }
  }
}
