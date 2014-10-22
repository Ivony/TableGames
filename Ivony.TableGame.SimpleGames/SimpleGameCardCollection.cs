using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGameCardCollection : CardSlotCollection
  {

    public SimpleGameCardCollection()
    {

      base.Slots.Add( new CardSlot<SimpleGameCard>( SimpleGame.AttackCardDealer ) );
      base.Slots.Add( new CardSlot<SimpleGameCard>( SimpleGame.AttackCardDealer ) );
      base.Slots.Add( new CardSlot<SimpleGameCard>( SimpleGame.AttackCardDealer ) );
      base.Slots.Add( new CardSlot<SimpleGameCard>( SimpleGame.AttackCardDealer ) );

      base.Slots.Add( new CardSlot<SimpleGameCard>( SimpleGame.SpecialCardDealer ) );
      base.Slots.Add( new CardSlot<SimpleGameCard>( SimpleGame.SpecialCardDealer ) );

    }
  }
}
