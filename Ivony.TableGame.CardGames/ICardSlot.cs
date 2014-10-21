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

  }
}
