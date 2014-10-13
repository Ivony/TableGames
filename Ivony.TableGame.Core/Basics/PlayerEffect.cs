using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{

  /// <summary>
  /// 玩家效果，效果作用于特定玩家
  /// </summary>
  public abstract class PlayerEffect : Effect
  {


    protected PlayerEffect( string name, string description, string shorthand ) : base( name, description, shorthand ) { }


    public BasicGamePlayer Player { get; private set; }


    internal void Applied( BasicGamePlayer player )
    {
      Player = player;
    }


    internal PlayerEffect[] GetMutexInternal( PlayerEffect[] effects )
    {
      return GetMutexEffects( effects ).Cast<PlayerEffect>().ToArray();
    }

  }
}
