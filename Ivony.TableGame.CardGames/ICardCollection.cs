using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 定义一个卡牌容器
  /// </summary>
  public interface ICardCollection : IEnumerable<Card>
  {

    /// <summary>
    /// 添加一张卡牌
    /// </summary>
    /// <param name="card">要添加的卡牌</param>
    /// <returns>是否添加成功</returns>
    bool AddCard( Card card );


    /// <summary>
    /// 移除一张卡牌
    /// </summary>
    /// <param name="card">要移除的卡牌</param>
    /// <returns>是否移除成功</returns>
    bool RemoveCard( Card card );


    /// <summary>
    /// 卡牌数量
    /// </summary>
    int Count { get; }


    /// <summary>
    /// 确定是否存在指定的卡牌
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    bool Contains( Card card );


    /// <summary>
    /// 清除所有卡牌
    /// </summary>
    void Clear();
  }


  public interface ICardCollection<in TCard> : ICardCollection where TCard : Card
  {

    /// <summary>
    /// 添加一张卡牌
    /// </summary>
    /// <param name="card">要添加的卡牌</param>
    /// <returns>是否添加成功</returns>
    bool AddCard( TCard card );


    /// <summary>
    /// 移除一张卡牌
    /// </summary>
    /// <param name="card">要移除的卡牌</param>
    /// <returns>是否移除成功</returns>
    bool RemoveCard( TCard card );


    /// <summary>
    /// 确定是否存在指定的卡牌
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    bool Contains( TCard card );

  }
}
