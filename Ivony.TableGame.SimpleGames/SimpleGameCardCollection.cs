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
      .Register( () => new AttackCard(), 100 )
      .Register( () => new ShieldCard(), 100 )
      .Register( () => new AngelCard(), 10 )
      .Register( () => new DevilCard(), 5 )
      .Register( () => new ReboundCard(), 20 )
      .Register( () => new PurifyCard(), 20 )
      ;

    public void DealCards()
    {
      DealCards( dealer, Math.Max( 5 - Count, 0 ) );
    }

  }
}
