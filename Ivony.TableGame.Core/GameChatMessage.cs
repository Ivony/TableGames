using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 代表一个玩家聊天消息
  /// </summary>
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

      Player = new PlayerInfo( player );

    }



    public class PlayerInfo
    {
      public PlayerInfo( GamePlayerBase player )
      {

        PlayerName = player.PlayerName;
        PlayerHostID = Guid.Empty;

      }



      public string PlayerName { get; private set; }


      public Guid PlayerHostID { get; private set; }


    }

    /// <summary>
    /// 发出消息的玩家
    /// </summary>
    public PlayerInfo Player { get; private set; }

  }
}
