using Ivony.TableGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 提供 IGameEvent 类型的基础实现
  /// </summary>
  public abstract class GameEventBase : IGameEvent
  {

    protected GameEventBase()
    {
      Data = new Dictionary<string, object>();
    }

    public IDictionary<string, object> Data { get; private set; }


  }
}
