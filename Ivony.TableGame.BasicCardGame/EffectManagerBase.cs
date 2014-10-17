using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.BasicCardGames
{
  public abstract class EffectManagerBase<TPlayer, TCard, TEffect, TGameEvent>
    where TPlayer : BasicGamePlayer<TCard>
    where TCard : BasicCard
    where TEffect : IEffect
  {

    public EffectManagerBase( BasicGame<TPlayer, TCard> game )
    {
      Game = game;
    }

    public BasicGame<TPlayer, TCard> Game { get; private set; }


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
