using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Ivony.TableGame.WebHost
{
  public class GameControllerBase : ApiController
  {

    /// <summary>
    /// 获取当前请求的玩家宿主对象
    /// </summary>
    protected PlayerHost PlayerHost
    {
      get
      {
        return (PlayerHost) ControllerContext.Request.Properties[PlayerHostHttpHandler.playerKey];
      }
    }

  }
}