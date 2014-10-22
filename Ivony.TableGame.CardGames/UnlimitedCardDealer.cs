using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 定义一个无限卡牌发牌器
  /// </summary>
  public class UnlimitedCardDealer<TCard> : CardDealer<TCard> where TCard : Card
  {

    private List<RegisteredCard> list = new List<RegisteredCard>();

    private class RegisteredCard
    {

      public RegisteredCard( Func<TCard> cardCreator, int probability )
      {
        CardCreator = cardCreator;
        Probability = probability;
      }

      public Func<TCard> CardCreator { get; private set; }

      public int Probability { get; private set; }
    }


    /// <summary>
    /// 注册一种卡牌
    /// </summary>
    /// <typeparam name="T">要注册的卡牌类型</typeparam>
    /// <param name="probability">卡牌出现的概率</param>
    public UnlimitedCardDealer<TCard> Register( Func<TCard> creator, int probability )
    {

      list.Add( new RegisteredCard( creator, probability ) );
      return this;
    }


    /// <summary>
    /// 发牌
    /// </summary>
    /// <returns>发出的牌</returns>
    public override TCard DealCard()
    {
      var n = Random.Next( list.Sum( item => item.Probability ) );

      foreach ( var item in list )
      {
        n -= item.Probability;
        if ( n < 0 )
          return item.CardCreator();
      }

      throw new InvalidOperationException();
    }
  }
}
