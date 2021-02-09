using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ivony.TableGame.WebHost
{
  public class PlayerHostMiddleware : IMiddleware
  {


    private static readonly string userTokenKey = "user-token";
    private static readonly string messageIndexKey = "message-index";

    public async Task InvokeAsync( HttpContext context, RequestDelegate next )
    {

      PlayerHost player;
      var userToken = context.Request.Cookies[userTokenKey];
      Guid userId;
      if ( userToken == null || !Guid.TryParse( userToken, out userId ) )
        player = Players.CreatePlayerHost();

      else
        player = Players.GetPlayerHost( userId ) ?? Players.CreatePlayerHost();


      int messageIndex;
      if ( int.TryParse( context.Request.Cookies[messageIndexKey], out messageIndex ) )
        player.SetMessageIndex( messageIndex );

      context.Features.Set( player );


      await next( context );


      context.Response.Cookies.Append( userTokenKey, player.ID.ToString(), new CookieOptions { Path = context.Request.PathBase } );
      context.Response.Cookies.Append( messageIndexKey, player.LastMesageIndex.ToString(), new CookieOptions { Path = context.Request.PathBase } );
    }
  }
}