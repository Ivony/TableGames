using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{


  /// <summary>
  /// 实现一个基于卡牌槽位的卡牌容器
  /// </summary>
  public abstract class CardSlotCollection : ICardCollection
  {

    /// <summary>
    /// 获取当前所有的卡牌槽位
    /// </summary>
    protected List<ICardSlot> Slots { get; private set; }

    /// <summary>
    /// 创建 CardSlotCollection 对象
    /// </summary>
    protected CardSlotCollection()
    {
      SyncRoot = new object();
      Slots = new List<ICardSlot>();
    }

    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    public object SyncRoot { get; private set; }

    /// <summary>
    /// 添加一张卡牌
    /// </summary>
    /// <param name="card">要添加的卡牌</param>
    /// <returns>是否添加成功</returns>
    public bool AddCard( Card card )
    {
      if ( card == null )
        throw new ArgumentNullException( "card" );

      lock ( SyncRoot )
      {
        var emptySlots = Slots.Where( item => item.HasCard == false ).ToArray();
        return emptySlots.Any( item => item.SetCard( card ) );
      }
    }


    /// <summary>
    /// 移除一张卡牌
    /// </summary>
    /// <param name="card">要移除的卡牌</param>
    /// <returns>是否移除成功</returns>
    public bool RemoveCard( Card card )
    {
      if ( card == null )
        throw new ArgumentNullException( "card" );

      lock ( SyncRoot )
      {
        return Slots.Any( item => card.Equals( item.Card ) && item.RemoveCard() );
      }
    }


    /// <summary>
    /// 当前卡牌数量
    /// </summary>
    public int Count
    {
      get
      {
        lock ( SyncRoot )
        {
          return Slots.Count( item => item.Card != null );
        }
      }
    }


    /// <summary>
    /// 获取是否存在某张卡牌
    /// </summary>
    /// <param name="card">要检测的卡牌对象</param>
    /// <returns>是否存在这张卡牌</returns>
    public bool Contains( Card card )
    {
      if ( card == null )
        throw new ArgumentNullException( "card" );

      lock ( SyncRoot )
      {
        return Slots.Any( item => card.Equals( item.Card ) );
      }
    }


    /// <summary>
    /// 清空所有卡牌
    /// </summary>
    public void Clear()
    {
      lock ( SyncRoot )
      {
        foreach ( var item in Slots.Where( item => item.HasCard ) )
          item.RemoveCard();
      }
    }

    IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
    {
      return Slots.Where( item => item.Card != null ).Select( item => item.Card ).GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<Card>) this).GetEnumerator();
    }


    public void DealCards()
    {
      lock ( SyncRoot )
      {
        foreach ( var item in Slots.Where( item => item.HasCard == false ) )
          item.DealCard();
      }
    }
  }
}
