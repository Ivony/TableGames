using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ivony.Data;
using System.Web.Http.ModelBinding;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Ivony.TableGame.WebHost
{


  /// <summary>
  /// 玩家宿主，登陆用户在系统中的宿主对象
  /// </summary>
  public class PlayerHost : IPlayerHost
  {


    public Guid Guid
    {
      get;
      private set;
    }



    private PlayerHost( Guid id )
    {
      Guid = id;
      _console = new PlayerConsole( this );
    }


    public static PlayerHost CreatePlayerHost()
    {

      lock ( _sync )
      {

        var host = new PlayerHost( Guid.NewGuid() );
        hosts.Add( host.Guid, host );
        return host;

      }
    }

    private static object _sync = new object();
    private static Hashtable hosts = new Hashtable();


    public static PlayerHost GetPlayerHost( Guid userId )
    {
      lock ( _sync )
      {
        return hosts[userId] as PlayerHost;
      }

    }


    private PlayerConsole _console;

    /// <summary>
    /// 获取玩家控制台，用于给玩家显示消息
    /// </summary>
    public PlayerConsoleBase Console
    {
      get { return _console; }
    }



    private class PlayerConsole : PlayerConsoleBase
    {

      public PlayerHost PlayerHost { get; private set; }

      public PlayerConsole( PlayerHost host )
      {
        PlayerHost = host;
      }

      public override void WriteMessage( GameMessage message )
      {
        PlayerHost._messages.Add( message );
      }
    }


    private List<GameMessage> _messages = new List<GameMessage>();

    public GameMessage[] GetMessages()
    {
      return _messages.ToArray();
    }



    public static HttpParameterBinding GetBinding( HttpParameterDescriptor parameterDescriptor )
    {


      var type = parameterDescriptor.ParameterType;

      if ( type != typeof( PlayerHost ) && type != typeof( IPlayerHost ) )
        return null;

      return new ModelBinderParameterBinding( parameterDescriptor, new PlayHostBinder(), parameterDescriptor.Configuration.Services.GetValueProviderFactories() );
    }

    private class PlayHostBinder : IModelBinder
    {


      private static readonly string parameterName = "userId";

      public bool BindModel( HttpActionContext actionContext, ModelBindingContext bindingContext )
      {
        var type = bindingContext.ModelType;
        if ( type != typeof( PlayerHost ) && type != typeof( IPlayerHost ) )
          return false;

        if ( !bindingContext.ValueProvider.ContainsPrefix( parameterName ) )
          return false;

        Guid userId;

        if ( !Guid.TryParse( bindingContext.ValueProvider.GetValue( parameterName ).AttemptedValue, out userId ) )
          return false;


        bindingContext.Model = PlayerHost.GetPlayerHost( userId );
        return true;
      }
    }


  }
}
