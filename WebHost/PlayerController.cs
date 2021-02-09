using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ivony.TableGame.WebHost
{
  public class PlayerController : ControllerBase
  {



    [HttpGet]
    public object Name()
    {
      return HttpContext.Features.Get<PlayerHost>().Name;
    }

    [HttpGet]
    [HttpPost]
    public object Name( string name )
    {
      if ( HttpContext.Features.Get<PlayerHost>().TrySetName( name ) )
        return new HttpResponseMessage( HttpStatusCode.OK );

      else
        return new HttpResponseMessage( HttpStatusCode.Forbidden );
    }

  }
}