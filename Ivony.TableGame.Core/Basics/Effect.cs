using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{

  /// <summary>
  /// 代表一个持续的效果
  /// </summary>
  public abstract class Effect
  {

    protected Effect( string name, string description, string shorthand )
    {
      Name = name;
      Description = description;
      Shorthand = shorthand;
    }



    public abstract bool IsAvailable { get; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public string Shorthand { get; private set; }



    protected virtual Effect[] GetMutexEffects( Effect[] effects )
    {
      return new Effect[0];
    }

  }
}
