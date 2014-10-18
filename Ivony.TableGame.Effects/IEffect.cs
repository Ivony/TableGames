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



  public interface IGameBehaviorEffect : IEffect
  {
    Task OnBehaviorInitiator( IGameBehaviorEvent gameEvent );
    
    Task OnBehaviorRecipient( IGameBehaviorEvent gameEvent );

  }


  public interface IGamePlayerEffect : IEffect
  {

    Task OnGamePlayerEvent( IGamePlayerEvent gameEvent );

  }

}
