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
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Runtime.Remoting.Messaging;

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



    /// <summary>
    /// 若已经加入某个游戏，则获取游戏中的玩家对象
    /// </summary>
    public GamePlayer Player { get; private set; }


    public GamePlayer GetPlayer()
    {
      return Player;
    }

    /// <summary>
    /// 玩家已经加入游戏
    /// </summary>
    /// <param name="player"></param>
    public void JoinedGame( GamePlayer player )
    {

      lock ( _sync )
      {
        if ( Player != null )
          throw new InvalidOperationException( "玩家当前已经在另一个游戏，无法加入游戏" );

        Player = player;
      }
    }


    /// <summary>
    /// 玩家已经从游戏中释放
    /// </summary>
    public void LeavedGame()
    {
      lock ( _sync )
      {
        Player = null;
      }
    }



    /// <summary>
    /// 获取是否正在游戏
    /// </summary>
    public bool Gaming
    {
      get { return Player != null; }
    }


    /// <summary>
    /// 获取是否正在等待玩家响应
    /// </summary>
    public bool WaitForResponse
    {
      get { return _responding != null; }
    }



    private Responding _responding;

    private void SetResponding( Responding responding )
    {
      lock ( _sync )
      {
        if ( _responding != null && _responding.Canceled == false )
          throw new InvalidOperationException();

        _responding = responding;
      }
    }


    public void Response( string message )
    {

      lock ( _sync )
      {
        if ( _responding == null )
        {
          this.WriteSystemMessage( "未在响应窗口时间或已经超时，无法再接收消息" );
          return;
        }


        _responding.OnResponse( message );
        _responding = null;

      }

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

      public override async Task<string> ReadLine( string prompt )
      {
        WriteMessage( new SystemMessage( prompt ) );

        return await WaitResponse().ConfigureAwait( false );
      }

      private async Task<string> WaitResponse()
      {

        var responding = new Responding();
        PlayerHost.SetResponding( responding );

        return await responding.Task;


      }
    }



    private class Responding
    {

      private TaskCompletionSource<string> taskSource = new TaskCompletionSource<string>();

      public Responding()
      {

        System.Threading.Tasks.Task.Run( () =>
        {

          Thread.Sleep( new TimeSpan( 0, 1, 0 ) );
          taskSource.TrySetCanceled();
          Canceled = true;
        } );


      }
      public bool Canceled { get; private set; }


      public Task<string> Task { get { return taskSource.Task; } }

      public void OnResponse( string message )
      {
        taskSource.SetResult( message );
      }
    }





    private List<GameMessage> _messages = new List<GameMessage>();


    private int index = 0;

    internal void SetMessageIndex( int messageIndex )
    {
      index = messageIndex;
    }


    internal int LastMesageIndex
    {
      get;
      private set;
    }

    public GameMessage[] GetMessages()
    {
      LastMesageIndex = _messages.Count;
      return _messages.GetRange( index, LastMesageIndex - index ).ToArray();
    }



    public override string ToString()
    {
      return Guid.ToString();
    }


  }




}
