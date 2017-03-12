using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;
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




      attack = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 20, () => new AttackCard( 1 ) )
        .Register( 3, () => new AttackCard( 2 ) )
        .Register( 1, () => new AttackCard( 3 ) );

      RegisterSlot( attack );
      RegisterSlot( new UnlimitedCardDealer<ShieldCard>().Register( 1, () => new ShieldCard() ) );



      var healing = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 20, () => new HealingCard( 2 ) )
        .Register( 2, () => new HealingCard( 3 ) )
        .Register( 1, () => new HealingCard( 5 ) );

      var elements = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 1, () => new ElementCard( Element.金 ) )
        .Register( 1, () => new ElementCard( Element.木 ) )
        .Register( 1, () => new ElementCard( Element.水 ) )
        .Register( 1, () => new ElementCard( Element.火 ) )
        .Register( 1, () => new ElementCard( Element.土 ) );


      var specials = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 1, () => new CurseCard() )
        .Register( 1, () => new BlessCard() )
        .Register( 2, () => new PeepCard() )
        .Register( 3, () => new StealCard() )
        .Register( 2, () => new ExchangeCard() );


      var others = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 1, healing )
        .Register( 2, specials )
        .Register( 3, elements );

      RegisterSlot<SimpleGameCard>( others );
      RegisterSlot<SimpleGameCard>( others );
      RegisterSlot<SimpleGameCard>( others );

    }

    private ICardDealer<SimpleGameCard> attack;
  }
}
