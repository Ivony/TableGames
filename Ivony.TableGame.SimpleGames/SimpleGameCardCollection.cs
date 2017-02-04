using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGameCardCollection : CardCollection<SimpleGameCard>
  {

    private ICardDealer<SimpleGameCard> dealer = new UnlimitedCardDealer<SimpleGameCard>()
      .Register( () => new AttackCard( 1 ), 150 )
      .Register( () => new AttackCard( 2 ), 30 )
      .Register( () => new AttackCard( 3 ), 5 )
      .Register( () => new ShieldCard(), 150 )

      .Register( () => new HealingCard( 2 ), 10 )
      .Register( () => new HealingCard( 3 ), 3 )
      .Register( () => new HealingCard( 5 ), 1 )
      .Register( () => new PeepCard(), 10 )
      .Register( () => new StealCard(), 20 )
      .Register( () => new DiscardCard(), 15 )
      .Register( () => new ExchangeCard(), 15 )
      .Register( () => new ElementCard( Element.金 ), 20 )
      .Register( () => new ElementCard( Element.木 ), 20 )
      .Register( () => new ElementCard( Element.水 ), 20 )
      .Register( () => new ElementCard( Element.火 ), 20 )
      .Register( () => new ElementCard( Element.土 ), 20 )
      ;

    public void DealCards()
    {
      DealCards( dealer, Math.Max( 7 - Count, 0 ) );
    }

  }
}
