using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TwelveCards.WebHost
{
  public class GameController : ApiController
  {

    public object SignIn()
    {

      var host = CreatePlayerHost();
      return new
      {
        ID = host.Guid
      };

    }

    private PlayerHost CreatePlayerHost()
    {
      return PlayerHost.CreatePlayerHost();
    }

  }
}