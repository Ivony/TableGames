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
  public class UnlimitedCardDealer<TCard> : ICardDealer<TCard> where TCard : Card
  {


    /// <summary>
    /// 创建无限卡牌发牌器
    /// </summary>
    public UnlimitedCardDealer()
    {
      Random = new Random( DateTime.Now.Millisecond );
    }


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
    /// <param name="creator">创建卡牌的方法</param>
    public UnlimitedCardDealer<TCard> Register( int probability, Func<TCard> creator )
    {

      list.Add( new RegisteredCard( creator, probability ) );
      return this;
    }

    /// <summary>
    /// 注册一种卡牌
    /// </summary>
    /// <typeparam name="T">要注册的卡牌类型</typeparam>
    /// <param name="probability">卡牌出现的概率</param>
    /// <param name="dealer">发牌器</param>
    /// <param name="defaultCreator">当发牌器无法发出牌时需要使用的创建卡牌的方法</param>
    public UnlimitedCardDealer<TCard> Register( int probability, ICardDealer<TCard> dealer, Func<TCard> defaultCreator = null )
    {

      if ( defaultCreator != null )
        Register( probability, () => dealer.DealCard() ?? defaultCreator() );

      else
        Register( probability, () => dealer.DealCard() );

      return this;
    }



    protected Random Random { get; private set; }


    /// <summary>
    /// 发牌
    /// </summary>
    /// <returns>发出的牌</returns>
    public TCard DealCard()
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
