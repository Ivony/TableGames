using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Effects
{
  public class GameEffectContext
  {

    public GameEffectContext( IGameEvent gameEvent )
    {
      GameEvent = gameEvent;
    }

    public IGameEvent GameEvent { get; private set; }


  }
}
