using Ivony.TableGame.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.BasicCardGames
{
  public abstract class GameEventBase : IGameEvent
  {

    protected GameEventBase()
    {
      Data = new Dictionary<string, object>();
    }

    public IDictionary<string, object> Data { get; private set; }


    public bool Handled
    {
      get { return false; }
    }
  }
}
