using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public class GameChatMessage : GameMessage
  {


    /// <summary>
    /// 创建一条 GameChatMessage 对象
    /// </summary>
    /// <param name="player">发出消息的玩家</param>
    /// <param name="message">消息内容</param>
    public GameChatMessage( GamePlayerBase player, string message )
      : base( GameMessageType.Chat, message )
    {

      Player = player;

    }


    /// <summary>
    /// 发出消息的玩家
    /// </summary>
    public GamePlayerBase Player { get; private set; }

  }
}
