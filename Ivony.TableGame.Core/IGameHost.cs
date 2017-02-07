using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{


  /// <summary>
  /// 定义游戏宿主
  /// </summary>
  public interface IGameHost
  {


    /// <summary>
    /// 游戏房间 ID
    /// </summary>
    Guid ID { get; }


    /// <summary>
    /// 游戏宿主名称
    /// </summary>
    string RoomName { get; }


    /// <summary>
    /// 游戏宿主数据
    /// </summary>
    Dictionary<string, object> Data { get; }


    /// <summary>
    /// 正在进行的游戏对象
    /// </summary>
    GameBase Game { get; }


    /// <summary>
    /// 尝试加入一个玩家
    /// </summary>
    /// <param name="player">要加入的玩家</param>
    /// <param name="reason">加入失败的原因</param>
    /// <returns>是否加入成功</returns>
    bool TryJoinGame( IPlayerHost player, out string reason );


    /// <summary>
    /// 尝试开始运行游戏
    /// </summary>
    /// <returns>返回一个 Task 对象，用于等待游戏结束</returns>
    Task Run();



    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    object SyncRoot { get; }


    /// <summary>
    /// 游戏结束后，调用此方法通知宿主释放游戏资源
    /// </summary>
    /// <param name="game">已经结束并释放资源的游戏对象</param>
    void ReleaseGame( GameBase game );



    /// <summary>
    /// 对游戏房间的玩家推送聊天消息
    /// </summary>
    /// <param name="message">消息内容</param>
    void SendChatMessage( GameChatMessage message );

  }
}
