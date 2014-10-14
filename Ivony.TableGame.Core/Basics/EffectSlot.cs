using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public abstract class EffectSlot
  {


    protected EffectSlot()
    {
      SyncRoot = new object();
    }


    public EffectBase Effect { get; protected set; }

    protected object SyncRoot { get; private set; }



    public virtual bool TrySetEffect( EffectBase effect )
    {
      if ( effect == null )
        throw new ArgumentNullException( "effect" );

      if ( !CanSetEffect( effect ) )
        return false;

      Effect = effect;
      return true;
    }

    public virtual bool CanSetEffect( EffectBase effect )
    {

      if ( effect == null )
        throw new ArgumentNullException( "effect" );

      return true;
    }

    public void Clear()
    {
      Effect = null;
    }

  }

  public class EffectSlot<T> : EffectSlot where T : EffectBase
  {
    public virtual bool TrySetEffect( EffectBase effect )
    {
      if ( effect == null )
        throw new ArgumentNullException( "effect" );


      var _effect = effect as T;
      return TrySetEffect( _effect );
    }

    public virtual bool TrySetEffect( T effect )
    {
      if ( !CanSetEffect( effect ) )
        return false;

      base.Effect = effect;
      return true;
    }




    public virtual bool CanSetEffect( EffectBase effect )
    {

      if ( effect == null )
        throw new ArgumentNullException( "effect" );


      var _effect = effect as T;
      return CanSetEffect( _effect );
    }


    public virtual bool CanSetEffect( T effect )
    {
      return true;
    }



    public new T Effect { get { return (T) base.Effect; } }

  }
}
