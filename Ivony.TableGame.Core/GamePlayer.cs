﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Ivony.TableGame
{
  public abstract class GamePlayer
  {

    protected GamePlayer( string codeName, IGameHost gameHost, IPlayerHost playerHost )
    {

      CodeName = codeName;
      PlayerHost = playerHost;
      GameHost = gameHost;

      CardCollection = new List<Card>();
    }



    /// <summary>
    /// 玩家代号
    /// </summary>
    public string CodeName { get; private set; }


    /// <summary>
    /// 游戏宿主对象，用于记录游戏的状态和
    /// </summary>
    public IGameHost GameHost { get; private set; }


    /// <summary>
    /// 玩家宿主对象，用于显示游戏信息和接受玩家输入
    /// </summary>
    public IPlayerHost PlayerHost { get; private set; }



    /// <summary>
    /// 玩家所持有的卡牌集合
    /// </summary>
    protected List<Card> CardCollection { get; private set; }


    /// <summary>
    /// 玩家所持有的卡牌
    /// </summary>
    public Card[] Cards { get { return CardCollection.ToArray(); } }



    public void WriteMessage( GameMessage message )
    {
      PlayerHost.Console.WriteMessage( message );
    }
    public void WriteMessage( string message )
    {
      WriteMessage( new GenericMessage( GameMessageType.Info, message ) );
    }

    public void WriteMessage( string format, params object[] args )
    {
      WriteMessage( string.Format( CultureInfo.InvariantCulture, format, args ) );
    }




    public abstract object GetGameInformation();



    /// <summary>
    /// 新增卡牌
    /// </summary>
    /// <param name="cards">新增的卡牌</param>
    public void AddCard( params Card[] cards )
    {
      CardCollection.AddRange( cards.Where( item => item != null ) );
    }
  }
}
