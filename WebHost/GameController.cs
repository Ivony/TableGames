using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ivony.TableGame.WebHost
{
  public class GameController : ApiController
  {

    [HttpGet]
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


    [HttpGet]
    public object CreateGame( PlayerHost playerHost )
    {

      if ( playerHost == null )
        throw new HttpResponseException( new HttpResponseMessage() { Content = new StringContent( "尚未登录" ), StatusCode = HttpStatusCode.Forbidden } );

      var game = new GameHost();
      game.Initialize();

      var result = game.TryJoinGame( playerHost );

      return "OK";
    }

    [HttpGet]
    public object GetMessages( PlayerHost playerHost )
    {
      return playerHost.GetMessages();
    }
  }
}