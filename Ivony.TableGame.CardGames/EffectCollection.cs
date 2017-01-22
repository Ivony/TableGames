using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义一个游戏效果的容器
  /// </summary>
  public class EffectCollection : IEffectCollection
  {

    /// <summary>
    /// 创建 EffectCollection 对象
    /// </summary>
    public EffectCollection()
    {
      SyncRoot = new object();
      Effects = new HashSet<IEffect>();
    }


    /// <summary>
    /// 当前存在的游戏效果
    /// </summary>
    protected HashSet<IEffect> Effects { get; private set; }


    /// <summary>
    /// 用于同步的对象
    /// </summary>
    public object SyncRoot { get; private set; }


    /// <summary>
    /// 添加一个游戏效果
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    public bool AddEffect( IEffect effect )
    {

      lock ( SyncRoot )
      {
        if ( !CanAddEffect( effect ) )
          return false;

        if ( Effects.Add( effect ) )
        {
          RemoveMutex( effect );
          return true;
        }

        else
          return false;
      }
    }

    /// <summary>
    /// 派生类实现此方法，判断效果是否可以附加
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    protected virtual bool CanAddEffect( IEffect effect )
    {
      return true;
    }

    /// <summary>
    /// 派生类实现此方法，移除互斥的效果
    /// </summary>
    /// <param name="effect"></param>
    protected virtual void RemoveMutex( IEffect effect )
    {
    }


    /// <summary>
    /// 移除一个效果
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    public bool RemoveEffect( IEffect effect )
    {
      return Effects.Remove( effect );
    }


    /// <summary>
    /// 获取效果数量
    /// </summary>
    public int Count
    {
      get { return Effects.Count; }
    }

    public bool Contains( IEffect effect )
    {
      return Effects.Contains( effect );
    }

    public void ClearEffect()
    {
      Effects.Clear();
    }

    public IEnumerator<IEffect> GetEnumerator()
    {
      return Effects.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
