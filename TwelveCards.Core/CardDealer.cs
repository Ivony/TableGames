using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards
{

  /// <summary>
  /// 定义一个发牌机，按照指定概率随机发牌
  /// </summary>
  public sealed class CardDealer
  {



    private List<RegisteredCard> list = new List<RegisteredCard>();

    private class RegisteredCard
    {

      public RegisteredCard( Func<Card> cardCreator, int probability )
      {
        CardCreator = cardCreator;
        Probability = probability;
      }

      public Func<Card> CardCreator { get; private set; }

      public int Probability { get; private set; }
    }


    /// <summary>
    /// 注册一种卡牌
    /// </summary>
    /// <typeparam name="T">要注册的卡牌类型</typeparam>
    /// <param name="probability">出现概率</param>
    public void RegisterCard<T>( Func<T> creator, int probability ) where T : Card
    {

      list.Add( new RegisteredCard( creator, probability ) );

    }

    Random random = new Random( DateTime.Now.Millisecond );

    /// <summary>
    /// 随机发一张卡牌
    /// </summary>
    /// <returns>发出的卡牌</returns>
    public Card Deal()
    {
      var n = random.Next( list.Sum( item => item.Probability ) );

      foreach ( var item in list )
      {
        n -= item.Probability;
        if ( n <= 0 )
          return item.CardCreator();
      }

      throw new InvalidOperationException();
    }


  }
}
