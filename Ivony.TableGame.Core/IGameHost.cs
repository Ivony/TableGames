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

    GameState GameState { get; }


    Game Game { get; }


    bool TryJoinGame( IPlayerHost player, out string reason );


    Task Run();


    object SyncRoot { get; }

  }
}
