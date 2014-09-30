using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwelveCards.WebHost
{
  public class PlayerHost : IPlayerHost
  {


    public Guid Guid
    {
      get;
      private set;
    }



    private PlayerHost( Guid guid )
    {
      Guid = guid;
    }


    public PlayerHost CreatePlayerHost()
    {

      return new PlayerHost( Guid.NewGuid() );

    }

    public PlayerConsole PlayerConsole
    {
      get { throw new NotImplementedException(); }
    }
  }
}
