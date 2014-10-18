using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Effects
{
  public abstract class EffectManagerBase<TPlayer, TEffect, TGameEvent>
    where TPlayer : GamePlayerBase
    where TEffect : IEffect
  {

    public EffectManagerBase( GameBase game )
    {
      Game = game;
    }

    public GameBase Game { get; private set; }


    public virtual void OnGameEvent( IGameEvent gameEvent )
    {
      OnGameEvent( gameEvent, GetAllEffects() );
    }

    protected virtual void OnGameEvent( IGameEvent gameEvent, TEffect[] effects )
    {

      var playerEvent = gameEvent as IGamePlayerEvent;

      foreach ( var item in effects.OfType<IInitiatorEffect>() )
      {
        item.OnPlayerEvent( playerEvent );
      }

    }


    protected virtual TEffect[] GetAllEffects()
    {
      return new TEffect[0];
    }


    public bool TryAddEffect( TEffect effect )
    {
      return false;
    }

    public bool TryAddEffect( TEffect effect, TPlayer player )
    {
      return false;
    }





  }
}
