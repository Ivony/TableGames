using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 定义卡牌槽位
  /// </summary>
  public interface ICardSlot
  {

    /// <summary>
    /// 槽位中所存在的卡牌
    /// </summary>
    Card Card { get; }

    /// <summary>
    /// 设置一张卡牌
    /// </summary>
    /// <param name="card">要设置的卡牌</param>
    /// <returns>是否设置成功</returns>
    bool SetCard( Card card );

    /// <summary>
    /// 通知发牌器给该槽位发一张牌
    /// </summary>
    /// <returns></returns>
    bool DealCard();

    /// <summary>
    /// 移除该槽位的卡牌
    /// </summary>
    /// <returns>是否成功</returns>
    bool RemoveCard();


    /// <summary>
    /// 获取该槽位是否有卡牌
    /// </summary>
    bool HasCard { get; }

  }




  /// <summary>
  /// 定义一个存放指定类型卡牌的卡牌槽位
  /// </summary>
  /// <typeparam name="TCard"></typeparam>
  public class CardSlot<TCard> : ICardSlot where TCard : Card
  {

    /// <summary>
    /// 发牌器
    /// </summary>
    protected CardDealer<TCard> CardDealer { get; private set; }

    /// <summary>
    /// 创建 CardSlot 对象
    /// </summary>
    /// <param name="dealer"></param>
    public CardSlot( CardDealer<TCard> dealer )
    {
      CardDealer = dealer;
      SyncRoot = new object();
    }


    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    public object SyncRoot { get; private set; }


    /// <summary>
    /// 槽位中所放的卡牌
    /// </summary>
    public Card Card
    {
      get;
      protected set;
    }


    /// <summary>
    /// 为槽位设置一张卡牌（调用此方法将抛出异常）
    /// </summary>
    /// <param name="card">要设置的卡牌</param>
    /// <returns>永远抛出异常</returns>
    public bool SetCard( Card card )
    {
      throw new InvalidOperationException();
    }

    /// <summary>
    /// 通知发牌器给该卡牌槽位发牌
    /// </summary>
    /// <returns></returns>
    public bool DealCard()
    {
      lock ( SyncRoot )
      {

        if ( HasCard )
          return false;


        Card = CardDealer.DealCard();
        return true;

      }
    }

    /// <summary>
    /// 移除卡牌
    /// </summary>
    /// <returns>是否移除成功</returns>
    public bool RemoveCard()
    {
      lock ( SyncRoot )
      {
        if ( !HasCard )
          return false;

        Card = null;
        return true;
      }
    }


    /// <summary>
    /// 槽位中是否存在卡牌
    /// </summary>
    public bool HasCard
    {
      get { return Card != null; }
    }
  }
}
