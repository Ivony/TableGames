using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ivony.TableGame
{
  public sealed class Player
  {

    internal Player( string codeName, Cabin cabin, IPlayerHost host )
    {

      CodeName = codeName;
      Cabin = cabin;
      Host = host;

    }



    public Game Game { get { return Cabin.Game; } }

    /// <summary>
    /// 玩家所处的舱位
    /// </summary>
    public Cabin Cabin { get; private set; }


    /// <summary>
    /// 玩家代号
    /// </summary>
    public string CodeName { get; private set; }

    /// <summary>
    /// 玩家控制台，用于显示游戏信息
    /// </summary>
    public IPlayerHost Host { get; private set; }


    /// <summary>
    /// 玩家所持有的卡牌
    /// </summary>
    public Card[] Cards { get; private set; }



    public void WriteMessage( GameMessage message )
    {
      Host.Console.WriteMessage( message );
    }
    public void WriteMessage( string message )
    {
      WriteMessage( new GenericMessage( GameMessageType.Info, message ) );
    }

    public void WriteMessage( string format, params object[] args )
    {
      WriteMessage( string.Format( CultureInfo.InvariantCulture, format, args ) );
    }



  }
}
