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
  /// 标记接口，标记实现此接口的效果为正面效果
  /// </summary>
  public interface IPositiveEffect : IEffect
  {
  }


  /// <summary>
  /// 标记接口，标记实现此接口的效果为负面效果
  /// </summary>
  public interface INagativeEffect : IEffect
  {
  }

}
