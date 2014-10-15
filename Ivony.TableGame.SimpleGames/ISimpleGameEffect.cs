using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{

  public interface ISimpleGameEffect : IEffect
  {
    Task<bool> OnAttack( SimpleGamePlayer user, SimpleGamePlayer target, int point );

  }


  public interface IAroundEffect : ISimpleGameEffect
  {
    Task OnTurnedAround( SimpleGamePlayer player );
  }

  public interface ISpecialEffect : ISimpleGameEffect
  {
  }

  public interface IDefenceEffect : ISimpleGameEffect
  {
  }
}
