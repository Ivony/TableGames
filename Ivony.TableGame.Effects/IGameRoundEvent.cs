using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Effects
{
  public interface IGameRoundEvent
  {

    int CurrentRound { get; }

    GamePlayerBase CurrentPlayer { get; }

  }
}
