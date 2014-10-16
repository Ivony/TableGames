using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{


  /// <summary>
  /// 游戏宿主
  /// </summary>
  public interface IGameHost
  {


    string RoomName { get; }


    GameBase Game { get; }


    bool TryJoinGame( IPlayerHost player, out string reason );


    Task Run();


    object SyncRoot { get; }


    /// <summary>
    /// 游戏结束后，调用此方法通知宿主释放游戏资源
    /// </summary>
    /// <param name="game">已经结束并释放资源的游戏对象</param>
    void ReleaseGame( GameBase game );
  }
}
