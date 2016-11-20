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



    /// <summary>
    /// 发出聊天消息的玩家信息
    /// </summary>
    public class PlayerInfo
    {

      /// <summary>
      /// 创建 PlayerInfo 对象
      /// </summary>
      /// <param name="player"></param>
      public PlayerInfo( GamePlayerBase player )
      {

        PlayerName = player.PlayerName;
        PlayerHostID = player.PlayerHost.ID;

      }



      /// <summary>
      /// 在游戏中的玩家名称
      /// </summary>
      public string PlayerName { get; private set; }


      /// <summary>
      /// 玩家的唯一标识
      /// </summary>
      public Guid PlayerHostID { get; private set; }


    }

    /// <summary>
    /// 发出消息的玩家
    /// </summary>
    public PlayerInfo Player { get; private set; }

  }
}
