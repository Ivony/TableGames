using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Effects
{
  public abstract class EffectManagerBase
  {

    public EffectManagerBase( GameBase game )
    {
      Game = game;
      SyncRoot = new object();
    }

    public GameBase Game { get; private set; }


    public virtual async Task OnGameEvent( IGameEvent gameEvent )
    {

      var behaviorEvent = gameEvent as IGameBehaviorEvent;
      if ( behaviorEvent != null )
       await OnGameEvent( behaviorEvent );
    }

    protected virtual async Task OnGameEvent( IGameBehaviorEvent behaviorEvent )
    {
      IGameBehaviorEffect[] initiatorEffects;
      lock ( SyncRoot )
      {
        var player = (GamePlayerBase) behaviorEvent.InitiatePlayer;
        initiatorEffects = playerEffects[player].OfType<IGameBehaviorEffect>().ToArray();
      }

      foreach ( var item in initiatorEffects )
        await item.OnBehaviorInitiator( behaviorEvent );

      


      IGameBehaviorEffect[] recipientEffects;
      lock ( SyncRoot )
      {
        var player = (GamePlayerBase) behaviorEvent.RecipientPlayer;
        recipientEffects = playerEffects[player].OfType<IGameBehaviorEffect>().ToArray();
      }

      foreach ( var item in initiatorEffects )
        await item.OnBehaviorRecipient( behaviorEvent );
    }

    protected virtual void OnGameEvent( IGameEvent gameEvent, IEffect[] effects )
    {

      var playerEvent = gameEvent as IGameBehaviorEvent;



    }


    protected virtual IEffect[] GetAllEffects()
    {
      return new IEffect[0];
    }


    protected object SyncRoot { get; private set; }


    private EffectCollection globalEffects = new EffectCollection();
    private Dictionary<GamePlayerBase, EffectCollection> playerEffects = new Dictionary<GamePlayerBase, EffectCollection>();

    public virtual bool TryAddEffect( IEffect effect )
    {
      lock ( globalEffects.SyncRoot )
      {
        return globalEffects.TryAdd( effect );
      }
    }

    public virtual bool TryAddEffect( IEffect effect, GamePlayerBase player )
    {

      EffectCollection collection;
      lock ( SyncRoot )
      {
        if ( playerEffects.ContainsKey( player ) )
          collection = playerEffects[player];

        playerEffects.Add( player, collection = new EffectCollection() );
        return collection.TryAdd( effect );
      }
    }
  }
}
