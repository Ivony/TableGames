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

    protected GameMessage( GameMessageType type, string message ) : this( type, message, DateTime.UtcNow ) { }
    protected GameMessage( GameMessageType type, string message, DateTime date, Version minimumClientVersion = null )
    {
      Type = type;
      Message = message;
      Date = date;
      ClientVersion = minimumClientVersion;
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


    /// <summary>
    /// 最小支持客户端版本，高于此版本的客户端不显示此消息
    /// </summary>
    public Version ClientVersion { get; private set; }

  }


  public sealed class SystemMessage : GameMessage
  {
    public SystemMessage( string message ) : base( GameMessageType.System, message ) { }
  }


  public sealed class GenericMessage : GameMessage
  {
    public GenericMessage( GameMessageType type, string message ) : base( type, message ) { }
  }



  public enum GameMessageType
  {
    Info,
    Warning,
    Error,
    System,
    SystemError
  }

}
