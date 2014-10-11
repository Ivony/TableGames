using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 提供简便的写入游戏消息的扩展方法
  /// </summary>
  public static class GameMessageExtensions
  {

    public static void WriteMessage( this IPlayerHost playerHost, GameMessage message )
    {
      playerHost.Console.WriteMessage( message );
    }


    /// <summary>
    /// 向玩家控制台写入一条消息
    /// </summary>
    /// <param name="playerHost">玩家宿主对象</param>
    /// <param name="message">消息内容</param>
    public static void WriteMessage( this IPlayerHost playerHost, string message )
    {
      playerHost.Console.WriteMessage( new GenericMessage( GameMessageType.Info, message ) );
    }


    public static void WriteMessage( this IPlayerHost playerHost, string format, params object[] args )
    {
      WriteMessage( playerHost, string.Format( CultureInfo.InvariantCulture, format, args ) );
    }

    public static void WriteSystemMessage( this IPlayerHost playerHost, string message )
    {
      playerHost.Console.WriteMessage( new GenericMessage( GameMessageType.System, message ) );
    }

    public static void WriteSystemMessage( this IPlayerHost playerHost, string format, params object[] args )
    {
      WriteSystemMessage( playerHost, string.Format( CultureInfo.InvariantCulture, format, args ) );
    }

    public static void WriteWarningMessage( this IPlayerHost playerHost, string message )
    {
      playerHost.Console.WriteMessage( new GenericMessage( GameMessageType.Warning, message ) );
    }

    public static void WriteWarningMessage( this IPlayerHost playerHost, string format, params object[] args )
    {
      WriteWarningMessage( playerHost, string.Format( CultureInfo.InvariantCulture, format, args ) );
    }

  }
}
