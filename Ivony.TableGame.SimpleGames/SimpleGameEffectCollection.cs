using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivony.TableGame;
using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGamePlayerEffectCollection : EffectSlotCollection
  {




    private TypedEffectSlot<BuffEffect> _buffEffect = new TypedEffectSlot<BuffEffect>();
    private TypedEffectSlot<ConfineEffect> _confineEffectSlot = new TypedEffectSlot<ConfineEffect>();
    private TypedEffectSlot<ShieldEffect> _shieldEffectSlot = new TypedEffectSlot<ShieldEffect>();


    public SimpleGamePlayerEffectCollection()
    {
      RegisterSlot( _buffEffect, _shieldEffectSlot, _confineEffectSlot );
    }
  }
}
