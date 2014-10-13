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



    /// <summary>
    /// 是否允许在目标上叠加多个同类效果？
    /// </summary>
    public virtual bool AllowMultipleInstance { get { return false; } }

    /// <summary>
    /// 获取效果的互斥类型组，即该效果与哪些效果互斥？
    /// </summary>
    public virtual Type[] MutexGroup { get { return null; } }

  }
}
