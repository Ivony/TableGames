using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards
{

  /// <summary>
  /// 代表使用卡牌所带来的效果
  /// </summary>
  public abstract class EffectBase
  {
    public EffectBase( Player player, Cabin target )
    {
      Player = player;
      Target = target;
    }


    /// <summary>
    /// 使用卡牌的玩家
    /// </summary>
    protected Player Player { get; private set; }
    /// <summary>
    /// 卡牌的目标
    /// </summary>
    protected Cabin Target { get; private set; }
    /// <summary>
    /// 当前游戏对象
    /// </summary>
    protected Game Game { get { return Player.Game; } }


    /// <summary>
    /// 应用卡牌使用的效果
    /// </summary>
    public abstract void ApplyEffect();


    /// <summary>
    /// 发送游戏通告
    /// </summary>
    protected void Announce( string format, params object[] args )
    {

      Game.AnnounceMessage( string.Format( format, args ) );

    }


  }
}
