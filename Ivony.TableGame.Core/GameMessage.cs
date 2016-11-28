using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{



  /// <summary>
  /// 代表一条游戏消息
  /// </summary>
  public abstract class GameMessage
  {

    /// <summary>
    /// 创建 GameMessage 对象
    /// </summary>
    /// <param name="type">游戏消息类型</param>
    /// <param name="message">消息内容</param>
    protected GameMessage( GameMessageType type, string message ) : this( type, message, DateTime.UtcNow ) { }

    /// <summary>
    /// 创建 GameMessage 对象
    /// </summary>
    /// <param name="type">游戏消息类型</param>
    /// <param name="message">消息内容</param>
    /// <param name="date">消息创建时间</param>
    protected GameMessage( GameMessageType type, string message, DateTime date )
    {
      Type = type;
      Message = message;
      Date = date;
    }


    /// <summary>
    /// 消息类型
    /// </summary>
    public GameMessageType Type { get; private set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// 消息产生的时间
    /// </summary>
    public DateTime Date { get; private set; }
  }



  /// <summary>
  /// 系统消息
  /// </summary>
  public sealed class SystemMessage : GameMessage
  {
    /// <summary>
    /// 创建 SystemMassage 对象
    /// </summary>
    /// <param name="message"></param>
    public SystemMessage( string message ) : base( GameMessageType.System, message ) { }
  }


  /// <summary>
  /// 通用消息
  /// </summary>
  public sealed class GenericMessage : GameMessage
  {
    /// <summary>
    /// 创建 GenericMessage 对象
    /// </summary>
    /// <param name="type">消息类型</param>
    /// <param name="message">消息内容</param>
    public GenericMessage( GameMessageType type, string message ) : base( type, message ) { }
  }




  /// <summary>
  /// 定义消息类型枚举
  /// </summary>
  public enum GameMessageType
  {
    /// <summary>一般信息消息</summary>
    Info,
    /// <summary>警告消息</summary>
    Warning,
    /// <summary>错误消息</summary>
    Error,
    /// <summary>系统消息</summary>
    System,
    /// <summary>系统错误消息</summary>
    SystemError,
    /// <summary>聊天消息消息</summary>
    Chat,
  }

}
