using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Effects
{
  public interface IGameBehaviorEvent : IGameEvent
  {

    GamePlayerBase InitiatePlayer { get; }

    GamePlayerBase RecipientPlayer { get; }
  }
}
