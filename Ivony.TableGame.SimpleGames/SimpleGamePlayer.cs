using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

  }
}
