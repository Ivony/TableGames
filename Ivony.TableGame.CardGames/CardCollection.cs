using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 提供 ICardCollection 的一个标准实现
  /// </summary>
  public class CardCollection : ICardCollection
  {

    /// <summary>
    /// 创建 CardCollection 对象
    /// </summary>
    public CardCollection()
    {
      CardList = new List<Card>();
      SyncRoot = new object();
    }

    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    public object SyncRoot { get; private set; }

    /// <summary>
    /// 获取内部的卡牌容器
    /// </summary>
    protected List<Card> CardList { get; private set; }

    /// <summary>
    /// 尝试添加一个卡牌
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool AddCard( Card card )
    {
      lock ( SyncRoot )
      {
        if ( CardList.Contains( card ) )
          return false;

        CardList.Add( card );
        return true;
      }
    }

    /// <summary>
    /// 尝试移除一个卡牌
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool RemoveCard( Card card )
    {
      lock ( SyncRoot )
      {
        return CardList.Remove( card );
      }
    }

    /// <summary>
    /// 获取目前卡牌数量
    /// </summary>
    public int Count
    {
      get { return CardList.Count; }
    }


    /// <summary>
    /// 确定是否存在指定的卡牌
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool Contains( Card card )
    {
      lock ( SyncRoot )
      {
        return CardList.Contains( card );
      }
    }


    /// <summary>
    /// 清除所有的卡牌
    /// </summary>
    public void Clear()
    {
      lock ( SyncRoot )
      {
        CardList.Clear();
      }
    }

    public IEnumerator<Card> GetEnumerator()
    {
      return CardList.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
