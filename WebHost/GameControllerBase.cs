using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ivony.TableGame.WebHost;
using Microsoft.AspNetCore.Mvc;

namespace Ivony.TableGame.WebHost
{
  public class GameControllerBase : ControllerBase
  {

    protected PlayerHost PlayerHost => HttpContext.Features.Get<PlayerHost>();

  }
}
