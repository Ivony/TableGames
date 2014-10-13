using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{

  /// <summary>
  /// 全局效果，效果作用于全局
  /// </summary>
  public abstract class GlobalEffect : Effect
  {
    protected GlobalEffect( string name, string description, string rule ) : base( name, description, rule ) { }



    internal GlobalEffect[] GetMutexInternal( GlobalEffect[] effects )
    {
      return GetMutexEffects( effects ).Cast<GlobalEffect>().ToArray();
    }


  }
}
