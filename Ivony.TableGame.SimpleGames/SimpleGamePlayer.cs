﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGamePlayer : GamePlayer
  {

    public SimpleGamePlayer( string codeName, IGameHost gameHost, IPlayerHost playerHost )
      : base( codeName, gameHost, playerHost )
    {

      Health = 100;

    }



    /// <summary>
    /// 生命值
    /// </summary>
    public int Health
    {
      get;
      internal set;
    }


    public async Task Play()
    {

      var command = ParseCommand( await PlayerHost.Console.ReadLine( "请出牌" ) );

    }



    public override object GetGameInformation()
    {
      return null;
    }


    private PlayCommand ParseCommand( string commandText )
    {
      throw new NotImplementedException();
    }
  }
}
