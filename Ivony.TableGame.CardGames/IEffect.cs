using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 代表一个游戏效果
  /// </summary>
  public interface IEffect
  {

    /// <summary>
    /// 效果名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 效果描述
    /// </summary>
    string Description { get; }

  }



  /// <summary>
  /// 定义与游戏行为产生相互影响的效果
  /// </summary>
  public interface IGameBehaviorEffect : IEffect
  {
    /// <summary>
    /// 当玩家发起一个游戏行为的时候
    /// </summary>
    /// <param name="gameEvent">游戏行为事件</param>
    /// <returns></returns>
    Task OnBehaviorInitiator( IGameBehaviorEvent gameEvent );

    /// <summary>
    /// 当玩家成为一个游戏行为的接受方的时候
    /// </summary>
    /// <param name="gameEvent">游戏行为事件</param>
    /// <returns></returns>
    Task OnBehaviorRecipient( IGameBehaviorEvent gameEvent );

  }


  /// <summary>
  /// 定义一个与玩家事件产生相互影响的效果
  /// </summary>
  public interface IGamePlayerEffect : IEffect
  {

    /// <summary>
    /// 当游戏中产生了一个玩家事件的时候
    /// </summary>
    /// <param name="gameEvent">玩家事件</param>
    /// <returns></returns>
    Task OnGamePlayerEvent( IGamePlayerEvent gameEvent );

  }

}
