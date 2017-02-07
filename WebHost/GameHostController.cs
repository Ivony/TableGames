using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace Ivony.TableGame.WebHost
{
  public class GameHostController : GameControllerBase
  {





    /// <summary>
    /// 强行释放用户退出系统
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public object Exit()
    {
      lock ( PlayerHost.SyncRoot )
      {
        PlayerHost.Release();
        return new HttpResponseMessage( HttpStatusCode.OK );
      }
    }




    [HttpGet]
    public object Status( HttpRequestMessage request, string messageMode = null )
    {

      int amount;
      if ( messageMode == "all" )
        PlayerHost.SetMessageIndex( 0 );

      else if ( int.TryParse( messageMode, out amount ) )
      {
        PlayerHost.GetMessages();
        PlayerHost.SetMessageIndex( Math.Max( PlayerHost.LastMesageIndex - amount, 0 ) );
      }


      return new
      {

        Gaming = PlayerHost.Gaming,
        RespondingUrl = PlayerHost.Responding == null ? null : "Responding/" + PlayerHost.Responding.RespondingID,

        GameInformation = GetGameInformation(),
        Messages = PlayerHost.GetMessages(),
      };
    }


    [HttpGet]

    public object Say( string message )
    {
      lock ( PlayerHost.SyncRoot )
      {
        var player = PlayerHost.Player;
        if ( player == null )
          throw new InvalidOperationException();


        player.GameHost.SendChatMessage( new GameChatMessage( player, message ) );
        return "OK";
      }
    }


    /// <summary>
    /// 客户端访问此方法获取游戏请求的特性列表
    /// </summary>
    /// <returns>游戏所需的特性</returns>
    [HttpGet]
    public string[] RequiredFeatures()
    {
      return PlayerHost.Player?.Game?.GetRequiredFeatures()?.ToArray();
    }



    private object GetGameInformation()
    {

      var player = PlayerHost.Player;

      if ( player == null )
        return null;

      return new
      {
        GameRoomName = player.GameHost.RoomName,
        Data = player.GetGameInformation(),
      };
    }


  }
}