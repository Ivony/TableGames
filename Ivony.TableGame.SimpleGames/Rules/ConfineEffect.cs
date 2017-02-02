using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  /// <summary>
  /// 禁锢效果
  /// </summary>
  public class ConfineEffect : SimpleGameEffect
  {
    public override string Description
    {
      get { return "被禁锢的玩家一个回合内不得进行任何操作"; }
    }

    public override string Name
    {
      get { return "禁锢"; }
    }

    public override string ToString()
    {
      return "禁";
    }
  }
}
