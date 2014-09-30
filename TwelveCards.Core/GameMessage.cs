using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards
{



  /// <summary>
  /// 代表一条游戏消息
  /// </summary>
  public abstract class GameMessage
  {

    protected GameMessage( GameMessageType type, string message ) : this( type, message, DateTime.UtcNow ) { }
    protected GameMessage( GameMessageType type, string message, DateTime date )
    {
      Type = type;
      Message = message;
      Date = date;
    }


    public GameMessageType Type { get; private set; }

    public string Message { get; private set; }

    public DateTime Date { get; private set; }

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
    System

  }

}
