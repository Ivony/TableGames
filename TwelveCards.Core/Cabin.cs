using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards.Core
{

  /// <summary>
  /// 代表一个舱位
  /// </summary>
  public sealed class Cabin
  {

    /// <summary>
    /// 所属的游戏
    /// </summary>
    public Game Game { get; private set; }

    /// <summary>
    /// 舱位所处的位置
    /// </summary>
    public int Index { get; private set; }

    /// <summary>
    /// 舱位的健康值
    /// </summary>
    public int Health { get; private set; }

    /// <summary>
    /// 舱位内所坐的玩家，如果有的话
    /// </summary>
    public Player Player { get; private set; }


    public void Damage( int points )
    {
      Health -= points;
    }

    public void Healing( int points )
    {
      Health += points;
    }

  }
}
