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
      .Register( () => new AttackCard( 1 ), 200 )
      .Register( () => new AttackCard( 2 ), 100 )
      .Register( () => new AttackCard( 3 ), 50 )
      .Register( () => new AttackCard( 4 ), 10 )
      .Register( () => new ShieldCard(), 200 )
      .Register( () => new ReboundCard(), 20 )
      .Register( () => new PurifyCard(), 20 )
      .Register( () => new PeepCard(), 10 )
      .Register( () => new ElementCard( Element.金 ), 10 )
      .Register( () => new ElementCard( Element.木 ), 10 )
      .Register( () => new ElementCard( Element.水 ), 10 )
      .Register( () => new ElementCard( Element.火 ), 10 )
      .Register( () => new ElementCard( Element.土 ), 10 )
      ;

    public void DealCards()
    {
      DealCards( dealer, Math.Max( 10 - Count, 0 ) );
    }

  }
}
