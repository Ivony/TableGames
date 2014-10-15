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
  public partial class PlayerHost : IPlayerHost
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

      lock ( globalSyncRoot )
      {

        var host = new PlayerHost( Guid.NewGuid() );
        hosts.Add( host.Guid, host );
        return host;

      }
    }

    private static object globalSyncRoot = new object();
    private static Hashtable hosts = new Hashtable();


    public static PlayerHost GetPlayerHost( Guid userId )
    {
      lock ( globalSyncRoot )
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

      lock ( globalSyncRoot )
      {
        if ( Player != null )
          throw new InvalidOperationException( "玩家当前已经在另一个游戏，无法加入游戏" );

        Player = player;
      }
    }


    /// <summary>
    /// 玩家已经从游戏中释放
    /// </summary>
    public void QuitGame()
    {
      lock ( SyncRoot )
      {
        if ( Player == null )
          throw new InvalidOperationException( "玩家当前未加入任何游戏，无法从游戏中退出" );

        Player.QuitGame();
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




    public object SyncRoot { get; private set; }


    internal IResponding Responding { get; set; }



    /// <summary>
    /// 获取是否正在等待玩家响应
    /// </summary>
    public bool WaitForResponse
    {
      get { return Responding != null; }
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
          if ( Responding == null )
            return null;

          else
            return Responding.PromptText;
        }
      }
    }



    internal void Response( string message )
    {

      lock ( SyncRoot )
      {
        if ( Responding == null )
        {
          this.WriteSystemMessage( "未在响应窗口时间或已经超时，无法再接收消息" );
          return;
        }


        Responding.OnResponse( message );
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
        return await new TextMessageResponding( PlayerHost, prompt, token ).RespondingTask.ConfigureAwait( false );
      }



      public override async Task<IOption> Choose( string prompt, IOption[] options, CancellationToken token )
      {
        return await new OptionsResponding( PlayerHost, prompt, options, token ).RespondingTask.ConfigureAwait( false );
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



    public OptionEntity[] GetOptions()
    {
      lock ( SyncRoot )
      {
        var responding = Responding as OptionsResponding;

        if ( responding == null )
          return null;

        return responding.Options.Select( item => new OptionEntity( item ) ).ToArray();
      }
    }
  }



}
