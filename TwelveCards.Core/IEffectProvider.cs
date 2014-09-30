using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public interface IEffectProvider
  {
    EffectBase GetEffect( Player player, Cabin target );

  }
}
