using Ivony.TableGame.CardGames;
using Ivony.TableGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{

  public interface IAroundEffect : IEffect
  {
    Task OnTurnedAround( SimpleGamePlayer player );
  }

  public interface IBlessEffect : IEffect
  {
  }

  public interface IDefenceEffect : IEffect
  {
  }
}
