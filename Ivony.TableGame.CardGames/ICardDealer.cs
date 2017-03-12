using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{


  /// <summary>
  /// 定义发牌器的抽象
  /// </summary>
  public interface ICardDealer<out TCard> where TCard : Card
  {
    TCard DealCard();

  }
}
