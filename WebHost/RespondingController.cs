using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ivony.TableGame.WebHost
{
  public class RespondingController : GameControllerBase
  {



    [HttpGet]
    [Route( "Responding/{id}" )]
    public object GetResponding( Guid id )
    {
      var responding = PlayerHost.Responding;

      if ( responding.RespondingID != id )
        return NotFound();

      return responding.GetInfo();
    }



    [HttpPost]
    [Route( "Responding/{id}" )]
    public async Task<object> PostResponding( Guid id )
    {
      var responding = PlayerHost.Responding;

      if ( responding == null )
        return NotFound();

      if ( Request.ContentType != "text/responding" )
        return BadRequest();


      var message = await new StreamReader( Request.Body ).ReadToEndAsync();
      PlayerHost.OnResponse( message );

      return "OK";
    }
  }
}