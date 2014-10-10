using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义一个无限卡牌发牌器
  /// </summary>
  public class UnlimitedCardDealer : CardDealer
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


    public override Card DealCard()
    {
      var n = Random.Next( list.Sum( item => item.Probability ) );

      foreach ( var item in list )
      {
        n -= item.Probability;
        if ( n <= 0 )
          return item.CardCreator();
      }

      throw new InvalidOperationException();
    }

    /// <summary>
    /// 注册空白卡牌概率
    /// </summary>
    /// <param name="probability">出现概率</param>
    public void RegisterBlankCard( int probability )
    {
      list.Add( new RegisteredCard( () => null, probability ) );
    }
  }
}
