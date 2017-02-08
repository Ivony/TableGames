using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义游戏玩家的抽象
  /// </summary>
  public abstract class GamePlayerBase
  {


    /// <summary>
    /// 创建 GamePlayerBase 对象
    /// </summary>
    /// <param name="gameHost">游戏宿主</param>
    /// <param name="playerHost">玩家宿主</param>
    protected GamePlayerBase( IGameHost gameHost, IPlayerHost playerHost )
    {
      PlayerHost = playerHost;
      GameHost = gameHost;
    }


    /// <summary>
    /// 获取一个随机数产生器
    /// </summary>
    protected static Random Random { get { return GameBase.Random; } }



    /// <summary>
    /// 玩家代号
    /// </summary>
    public string PlayerName { get { return PlayerHost.Name; } }


    /// <summary>
    /// 游戏宿主对象，用于记录游戏的状态和保存加载游戏
    /// </summary>
    public IGameHost GameHost { get; private set; }


    /// <summary>
    /// 获取游戏对象
    /// </summary>
    public GameBase Game { get { return GameHost.Game; } }

    /// <summary>
    /// 玩家宿主对象，用于显示游戏信息和接受玩家输入
    /// </summary>
    public IPlayerHost PlayerHost { get; private set; }



    /// <summary>
    /// 获取当前玩家可以看到的游戏信息
    /// </summary>
    /// <returns>游戏信息</returns>
    public abstract object GetGameInformation();


    /// <summary>
    /// 退出游戏
    /// </summary>
    public virtual void QuitGame()
    {
      PlayerHost = null;
    }



    /// <summary>
    /// 重写 ToString 方法，显示玩家信息
    /// </summary>
    /// <returns>玩家信息的字符串表达形式</returns>
    public override string ToString()
    {

      return string.Format( "{0} / {1}", PlayerHost.Name, GameHost.RoomName );
    }

  }
}
