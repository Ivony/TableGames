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
    public SimpleGameCardCollection()
    {

      var attack = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 20, () => new AttackCard( 1 ) )
        .Register( 3, () => new AttackCard( 2 ) )
        .Register( 1, () => new AttackCard( 3 ) );

      var healing = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 20, () => new HealingCard( 2 ) )
        .Register( 2, () => new HealingCard( 3 ) )
        .Register( 1, () => new HealingCard( 5 ) );

      basics = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 5, () => new ShieldCard() )
        .Register( 7, attack )
        .Register( 1, healing );


      var elements = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 1, () => new ElementCard( Element.金 ) )
        .Register( 1, () => new ElementCard( Element.木 ) )
        .Register( 1, () => new ElementCard( Element.水 ) )
        .Register( 1, () => new ElementCard( Element.火 ) )
        .Register( 1, () => new ElementCard( Element.土 ) );


      var specials = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 2, () => new PeepCard() )
        .Register( 3, () => new StealCard() )
        .Register( 4, () => new DiscardCard() )
        .Register( 3, () => new ExchangeCard() );


      all = new UnlimitedCardDealer<SimpleGameCard>()
        .Register( 20, basics )
        .Register( 2, specials )
        .Register( 10, elements );

    }

    private ICardDealer<SimpleGameCard> all;
    private ICardDealer<SimpleGameCard> basics;

    public void DealCards( SimpleGame game )
    {
      DealCards( all, Math.Max( 8 - Count, 0 ) );



      if ( game.Rounds == 1 && this.Any( item => item is ShieldCard ) == false )
      {//对于第一回合没有盾牌的玩家，强行发放两个盾牌
        AddCard( new ShieldCard() );
        AddCard( new ShieldCard() );
      }


      //如果牌组中缺少基本卡牌，则强行发放。
      var balance = Math.Max( 10 - Count, 0 );

      if ( this.Any( item => item is IBasicCard ) == false )
        DealCards( basics, balance );

      else
        DealCards( all, balance );



    }

  }
}
