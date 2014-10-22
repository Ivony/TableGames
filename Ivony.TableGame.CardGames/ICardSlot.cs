using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  public interface ICardSlot
  {

    Card Card { get; }

    bool SetCard( Card card );

    bool DealCard();

    bool RemoveCard();


    bool HasCard { get; }

  }



  public class CardSlot : ICardSlot
  {

    protected CardDealer CardDealer { get; private set; }

    public CardSlot( CardDealer dealer )
    {
      CardDealer = dealer;
      SyncRoot = new object();
    }


    public object SyncRoot { get; private set; }


    public Card Card
    {
      get;
      protected set;
    }

    public bool SetCard( Card card )
    {
      throw new InvalidOperationException();
    }

    public bool DealCard()
    {
      lock ( SyncRoot )
      {

        if ( HasCard )
          return false;


        Card = CardDealer.DealCard();
        return true;

      }
    }

    public bool RemoveCard()
    {
      lock ( SyncRoot )
      {
        if ( !HasCard )
          return false;

        Card = null;
        return true;
      }
    }


    public bool HasCard
    {
      get { return Card != null; }
    }
  }
}
