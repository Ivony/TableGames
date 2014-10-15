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
using System.Text.RegularExpressions;

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



    private PlayerHost( Guid id, string name )
    {
      Guid = id;
      Name = name;

      SyncRoot = new object();
      _console = new PlayerConsole( this );
    }



    public static PlayerHost CreatePlayerHost()
    {

      lock ( globalSyncRoot )
      {
        var instance = new PlayerHost( Guid.NewGuid(), PlayerNameManager.GetName() );
        playerHosts.Add( instance.Guid, instance );
        instance.ShowInitializeInfo();
        return instance;

      }
    }

    private void ShowInitializeInfo()
    {
      this.WriteSystemMessage( "欢迎您参与到通用卡牌游戏引擎实例游戏项目，您在游戏中的名字是 {0}，希望您能喜欢这个名字", Name );
      this.WriteSystemMessage( "通用卡牌游戏引擎可以帮助您快速的构建您想象中的卡牌游戏，其项目通过 Apache 2.0 协议开源。" );
      this.WriteSystemMessage( "项目地址： https://github.com/Ivony/TableGames ，作者： Ivony 。" );
      this.WriteSystemMessage( "特别感谢群友 @谁在秋千 对项目的大力支持和开发的 Web 游戏客户端。" );
      this.WriteSystemMessage( "记得点赞哦，，，，，，" );

    }

    private static object globalSyncRoot = new object();
    private static Hashtable playerHosts = new Hashtable();
    private static HashSet<string> playerNames = new HashSet<string>();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static PlayerHost GetPlayerHost( Guid userId )
    {
      lock ( globalSyncRoot )
      {
        return playerHosts[userId] as PlayerHost;
      }

    }



    /// <summary>
    /// 玩家名称
    /// </summary>
    public string Name
    {
      get;
      private set;
    }


    /// <summary>
    /// 尝试设置玩家名称
    /// </summary>
    /// <param name="name">玩家名称</param>
    public bool TrySetName( string name )
    {

      if ( !PlayerNameManager.ValidName( name ) )
      {
        this.WriteSystemMessage( "昵称不合法，昵称只能使用 3 - 10 位英文字母或者 2 - 5 个常见汉字" );
        return false;
      }
      if ( PlayerNameManager.CanReset( Name, name ) )
      {
        Name = name;
        this.WriteSystemMessage( "您的昵称已经成功更改为 {0}", Name );
        return true;
      }

      else
      {
        this.WriteSystemMessage( "更改名字失败，昵称已经被其他用户占用。" );
        return false;
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


    GamePlayer IPlayerHost.GetPlayer()
    {
      return Player;
    }

    /// <summary>
    /// 通知玩家已经加入了某个游戏
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
    /// 强行退出某个游戏
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




    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
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


    /// <summary>
    /// 获取需要玩家选择的选项信息（如果有的话）
    /// </summary>
    /// <returns>选项信息</returns>
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



    /// <summary>
    /// 当玩家响应了消息时，调用此方法
    /// </summary>
    /// <param name="message">响应的消息</param>
    internal void OnResponse( string message )
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

    /// <summary>
    /// 设置最后一次收取的消息位置
    /// </summary>
    /// <param name="messageIndex"></param>
    internal void SetMessageIndex( int messageIndex )
    {
      index = messageIndex;
    }


    /// <summary>
    /// 最后一次收取的消息位置
    /// </summary>
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
