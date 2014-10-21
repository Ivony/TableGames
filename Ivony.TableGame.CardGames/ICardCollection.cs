using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  public interface ICardCollection : IEnumerable<Card>
  {

    bool AddCard( Card card );


    bool RemoveCard( Card card );



    int Count { get; }


    bool Contains( Card card );


    void Clear();

  }
}
