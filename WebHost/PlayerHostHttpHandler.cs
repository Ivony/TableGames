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
    public static readonly string playerRouteKey = "player";

    protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
    {

      PlayerHost player;
      var userToken = request.Headers.GetCookies( userTokenKey ).Select( cookie => cookie.Cookies.FirstOrDefault() ).FirstOrDefault();

      Guid userId;
      if ( userToken == null || !Guid.TryParse( userToken.Value, out userId ) )
        player = PlayerHost.CreatePlayerHost();

      else
        player = PlayerHost.GetPlayerHost( userId ) ?? PlayerHost.CreatePlayerHost();

      request.GetRequestContext().RouteData.Values.Add( "player", player );


      var response = await base.SendAsync( request, cancellationToken );


      response.Headers.AddCookies( new[] { new CookieHeaderValue( userTokenKey, player.Guid.ToString() ) } );
      return response;
    }

  }
}