using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ivony.TableGame.WebHost
{
  public class PlayerController : GameControllerBase
  {

    [HttpGet]
    public object Name()
    {
      return PlayerHost.Name;
    }

    [HttpGet]
    [HttpPost]
    public object Name( string name )
    {
      if ( PlayerHost.TrySetName( name ) )
        return new HttpResponseMessage( HttpStatusCode.OK );

      else
        return new HttpResponseMessage( HttpStatusCode.Forbidden );
    }

  }
}