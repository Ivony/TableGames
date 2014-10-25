using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGameCardCollection : CardSlotCollection<SimpleGameCard>
  {

    public SimpleGameCardCollection()
    {

      AddSlot( SimpleGame.AttackCardDealer );
      AddSlot( SimpleGame.AttackCardDealer );
      AddSlot( SimpleGame.AttackCardDealer );
      AddSlot( SimpleGame.AttackCardDealer );

      AddSlot( SimpleGame.SpecialCardDealer );
      AddSlot( SimpleGame.SpecialCardDealer );

    }
  }
}
