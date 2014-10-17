using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.BasicCardGames
{
  public interface IGlobalEffectSlot
  {

    bool TryAddEffect( IEffect effect );



  }

  public interface IPlayerEffectSlot
  {

    bool TryAddEffect( IEffect effect, GamePlayerBase player );

  }
}
