using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义一个效果容器，可以添加和删除效果
  /// </summary>
  public interface IEffectCollection : IEnumerable<IEffect>
  {

    bool AddEffect( IEffect effect );


    bool RemoveEffect( IEffect effect );



    int Count { get; }


    bool Contains( IEffect effect );


    void ClearEffect();


  }
}
