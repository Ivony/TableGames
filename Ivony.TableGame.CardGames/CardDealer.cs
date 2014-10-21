using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 定义一个发牌机，按照指定概率随机发牌
  /// </summary>
  public abstract class CardDealer
  {


    protected CardDealer()
    {
      Random = new Random( DateTime.Now.Millisecond );

    }

    protected Random Random { get; private set; }



    /// <summary>
    /// 随机发一张卡牌
    /// </summary>
    /// <returns>发出的卡牌</returns>
    public abstract Card DealCard();


    public Card[] DealCards( int amount )
    {

      if ( amount <= 0 )
        return new Card[0];


      var cards = new Card[amount];
      for ( int i = 0; i < amount; i++ )
        cards[i] = DealCard();


      return cards;

    }
  }
}
