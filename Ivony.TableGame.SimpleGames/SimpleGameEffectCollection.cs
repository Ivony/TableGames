using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivony.TableGame;
using Ivony.TableGame.CardGames;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGamePlayerEffectCollection : EffectSlotCollection
  {



    public IBlessEffect BlessEffect { get { return _blessEffectSlot.Effect; } }
    public IDefenceEffect DefenceEffect { get { return _defenceEffectSlot.Effect; } }



    private TypedEffectSlot<IBlessEffect> _blessEffectSlot = new TypedEffectSlot<IBlessEffect>();
    private TypedEffectSlot<IDefenceEffect> _defenceEffectSlot = new TypedEffectSlot<IDefenceEffect>();


    public SimpleGamePlayerEffectCollection()
    {
      RegisterSlot( _blessEffectSlot, _defenceEffectSlot );
    }
  }
}
