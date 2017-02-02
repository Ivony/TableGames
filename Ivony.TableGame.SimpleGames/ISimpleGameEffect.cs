using Ivony.TableGame.CardGames;
using Ivony.TableGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{

  /// <summary>
  /// 定义一种回合结束时将发生变化的效果
  /// </summary>
  public interface IAroundEffect : IEffect
  {
    /// <summary>
    /// 当一个回合结束时
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    Task OnTurnedAround( SimpleGamePlayer player );
  }

  /// <summary>
  /// 定义一种增益效果
  /// </summary>
  public interface IBlessEffect : IEffect
  {
  }


  /// <summary>
  /// 定义一种防御效果
  /// </summary>
  public interface IDefenceEffect : IEffect
  {
  }

}
