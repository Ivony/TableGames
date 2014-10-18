using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Effects
{
  public interface IEffect
  {

    string Name { get; }

    string Description { get; }

  }



  public interface IInitiatorEffect : IEffect
  {

    void OnPlayerEvent( IGamePlayerEvent gameEvent );

    void OnBehaviorInitiator( IGameBehaviorEvent gameEvent );

  }

}
