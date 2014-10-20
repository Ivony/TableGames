using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivony.TableGame.Effects;

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

  }
}
