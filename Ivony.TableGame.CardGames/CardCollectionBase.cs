using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  public abstract class CardCollectionBase<TCard> : ICardCollection<TCard> where TCard : Card
  {



    bool ICardCollection.AddCard( Card card )
    {
      var _card = card as TCard;
      if ( _card == null )
        return false;

      return AddCard( _card );
    }

    bool ICardCollection.RemoveCard( Card card )
    {
      var _card = card as TCard;
      if ( _card == null )
        return false;

      return RemoveCard( _card );
    }

    bool ICardCollection.Contains( Card card )
    {
      var _card = card as TCard;
      if ( _card == null )
        return false;

      return Contains( _card );
    }


    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }



    public abstract bool AddCard( TCard card );

    public abstract bool RemoveCard( TCard card );

    public abstract bool Contains( TCard card );

    public abstract int Count { get; }

    public abstract void Clear();

    public abstract IEnumerator<Card> GetEnumerator();
  }
}
