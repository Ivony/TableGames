using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivony.TableGame;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGameEffectManager : EffectManagerBase
  {

    public SimpleGameEffectManager( SimpleGame game ) : base( game ) { }



    protected override void RemoveMutex( IEffect addedEffect, EffectCollection effects )
    {
      if ( addedEffect is IBlessEffect )
        effects.RemoveAll( effects.OfType<IBlessEffect>() );

      if ( addedEffect is IDefenceEffect )
        effects.RemoveAll( effects.OfType<IDefenceEffect>() );
    }

    protected override IEffect[] GetPlayerEffects( GamePlayerBase player )
    {
      return base.GetPlayerEffects( player ).OrderBy( item => GetOrder( item ) ).ToArray();
    }

    private int GetOrder( IEffect item )
    {
      if ( item is IBlessEffect )
        return 1;

      else if ( item is IDefenceEffect )
        return 2;

      else
        return 100;
    }

  }
}
