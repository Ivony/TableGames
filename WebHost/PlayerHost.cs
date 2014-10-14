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
      SyncRoot = new object();
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




    protected object SyncRoot { get; private set; }



    private Responding GetResponding()
    {

      return _responding;
    }


    /// <summary>
    /// 获取是否正在等待玩家响应
    /// </summary>
    public bool WaitForResponse
    {
      get { return _responding != null; }
    }



    /// <summary>
    /// 获取输入提示信息，当等待用户响应时，显示该提示信息给用户。
    /// </summary>
    public string PromptText
    {
      get
      {
        lock ( SyncRoot )
        {
          if ( _responding == null )
            return null;

          else
            return _responding.PromptText;
        }
      }
    }



    private Responding _responding;

    public void Response( string message )
    {

      lock ( SyncRoot )
      {
        if ( _responding == null )
        {
          this.WriteSystemMessage( "未在响应窗口时间或已经超时，无法再接收消息" );
          return;
        }


        _responding.OnResponse( message );

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

      public override async Task<string> ReadLine( string prompt, CancellationToken token )
      {
        return await WaitResponse( prompt, token ).ConfigureAwait( false );
      }

      private Task<string> WaitResponse( string prompt, CancellationToken token )
      {
        return Responding.CreateResponding( PlayerHost, prompt, token ).RespondingTask;
      }


      public override Task<IOption> Choose( string prompt, IOption[] options, CancellationToken token )
      {
        return ChooseResponding.CreateResponding( PlayerHost, prompt, options, token ).RespondingTask;
      }

    }



    private class Responding
    {

      private TaskCompletionSource<string> taskSource = new TaskCompletionSource<string>();



      public static Responding CreateResponding( PlayerHost host, string prompt, CancellationToken token )
      {
        lock ( host.SyncRoot )
        {
          if ( host._responding != null )
            throw new InvalidOperationException();

          return host._responding = new Responding( host, prompt, token );
        }

      }

      protected Responding( PlayerHost host, string prompt, CancellationToken token )
      {
        Host = host;
        PromptText = prompt;


        token.Register( () =>
        {
          taskSource.TrySetCanceled();
          lock ( Host.SyncRoot )
          {
            Host._responding = null;
          }
        } );
      }


      protected PlayerHost Host
      {
        get;
        private set;
      }


      public bool Canceled { get; private set; }


      public Task<string> RespondingTask { get { return taskSource.Task; } }


      public string PromptText { get; private set; }


      public virtual void OnResponse( string message )
      {
        lock ( Host.SyncRoot )
        {
          taskSource.TrySetResult( message );
          Host._responding = null;
        }
      }
    }



    private class ChooseResponding : Responding
    {

      private IOption[] _options;

      private ChooseResponding( PlayerHost host, string prompt, IOption[] options, CancellationToken token )
        : base( host, prompt, token )
      {
        _options = options;
      }

      public static ChooseResponding CreateResponding( PlayerHost host, string prompt, IOption[] options, CancellationToken token )
      {
        lock ( host.SyncRoot )
        {
          if ( host._responding != null )
            throw new InvalidOperationException();

          var responding = new ChooseResponding( host, prompt, options, token );
          host._responding = responding;
          return responding;
        }

      }


      private TaskCompletionSource<IOption> taskSource = new TaskCompletionSource<IOption>();


      public new Task<IOption> RespondingTask { get { return taskSource.Task; } }


      public override void OnResponse( string message )
      {

        IOption option;
        if ( !TryGetOption( message, out option ) )
        {
          Host.WriteWarningMessage( "您输入的格式不正确，应该输入 {0} - {1} 之间的数字", 1, _options.Length );
          return;
        }


        taskSource.TrySetResult( option );
        base.OnResponse( message );
      }

      private bool TryGetOption( string text, out IOption option )
      {
        option = null;
        int index;
        if ( !int.TryParse( text, out index ) )
          return false;

        if ( index < 1 || index > _options.Length )
          return false;

        option = _options[index - 1];
        return true;
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
      lock ( SyncRoot )
      {
        LastMesageIndex = _messages.Count;
        if ( index > LastMesageIndex )
          return new GameMessage[0];

        return _messages.GetRange( index, LastMesageIndex - index ).ToArray();
      }
    }



    public override string ToString()
    {
      return Guid.ToString();
    }


  }




}
