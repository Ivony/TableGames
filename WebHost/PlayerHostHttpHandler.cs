using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;


namespace Ivony.TableGame.WebHost
{
  public class PlayerHostHttpHandler : DelegatingHandler
  {


    public static readonly string userTokenKey = "user-token";
    public static readonly string playerKey = "player";

    protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
    {

      PlayerHost player;
      var userToken = request.Headers.GetCookieValue( userTokenKey );

      Guid userId;
      if ( userToken == null || !Guid.TryParse( userToken, out userId ) )
        player = PlayerHost.CreatePlayerHost();

      else
        player = PlayerHost.GetPlayerHost( userId ) ?? PlayerHost.CreatePlayerHost();

      request.Properties[playerKey] = player;


      var response = await base.SendAsync( request, cancellationToken );


      response.Headers.SetCookieValue( userTokenKey, player.Guid.ToString(), request.GetRequestContext().VirtualPathRoot );
      return response;
    }

  }
}