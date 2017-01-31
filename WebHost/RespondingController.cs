using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Ivony.TableGame.WebHost
{
  public class RespondingController : GameControllerBase
  {

    public override Task<HttpResponseMessage> ExecuteAsync( HttpControllerContext controllerContext, CancellationToken cancellationToken )
    {
      var routeValues = controllerContext.RouteData.Values;

      if ( controllerContext.Request.Method == HttpMethod.Get )
        routeValues["action"] = "GetResponding";

      else if ( controllerContext.Request.Method == HttpMethod.Post )
        routeValues["action"] = "PostResponding";

      else
        return NotFound().ExecuteAsync( cancellationToken );

      return base.ExecuteAsync( controllerContext, cancellationToken );
    }

    [HttpGet]
    public object GetResponding( Guid id )
    {
      var responding = PlayerHost.Responding;

      if ( responding.RespondingID != id )
        return NotFound();

      return responding.GetInfo();
    }



    [HttpPost]
    public async Task<object> PostResponding( Guid id )
    {
      var responding = GetResponding( id );

      if ( responding == null )
        return NotFound();

      if ( Request.Content.Headers.ContentType.MediaType != "text/responding" )
        return BadRequest();


      var message = await Request.Content.ReadAsStringAsync();
      PlayerHost.OnResponse( message );

      return "OK";
    }
  }
}