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
      RefreshTime = DateTime.UtcNow;
      _console = new PlayerConsole( this );
    }



    /// <summary>
    /// 上次状态刷新时间
    /// </summary>
    public DateTime RefreshTime { get; private set; }

    internal void RefreshState()
    {
      RefreshTime = DateTime.UtcNow;
    }


    /// <summary>
    /// 创建一个新的玩家宿主
    /// </summary>
    /// <returns>新的玩家宿主</returns>
    public static PlayerHost CreatePlayerHost()
    {

      lock ( globalSyncRoot )
      {
        var instance = new PlayerHost( Guid.NewGuid(), PlayerNameManager.CreateName() );
        playerHosts.Add( instance );
        instance.ShowInitializeInfo();
        return instance;

      }
    }

    private void ShowInitializeInfo()
    {
      this.WriteSystemMessage( "欢迎您参与到通用卡牌游戏引擎实例游戏项目，您在游戏中的昵称是 {0}，希望您能喜欢。", Name );
      this.WriteSystemMessage( "通用卡牌游戏引擎可以帮助您快速的构建您想象中的卡牌游戏，其项目通过 Apache 2.0 协议开源。" );
      this.WriteSystemMessage( "项目地址： https://github.com/Ivony/TableGames" );
      this.WriteSystemMessage( "特别感谢群友 @谁在秋千 对项目的大力支持和开发的 Web 游戏客户端。" );
      this.WriteSystemMessage( "记得点赞哦，，，，，，" );

    }

    private static object globalSyncRoot = new object();
    private static PlayerHostCollection playerHosts = new PlayerHostCollection();





    private static readonly TimeSpan playerHostTimeout = new TimeSpan( 0, 10, 0 );


    /// <summary>
    /// 尝试获取玩家宿主
    /// </summary>
    /// <param name="id">玩家宿主ID</param>
    /// <returns></returns>
    public static PlayerHost GetPlayerHost( Guid id )
    {
      lock ( globalSyncRoot )
      {

        var date = DateTime.UtcNow - playerHostTimeout;

        foreach ( var i in playerHosts.Where( item => item.RefreshTime < date ).ToArray() )
          i.Release();

        if ( playerHosts.Contains( id ) )
        {
          var instance = playerHosts[id];
          instance.RefreshState();
          return instance;
        }
        else
          return null;
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
        if ( Player != null )
          Player.GameHost.Game.AnnounceSystemMessage( "玩家 {0} 已经更名为 {1}", Name, name );

        Name = name;
        this.WriteSystemMessage( "您的昵称已经成功更改为 {0}", Name );

        return true;
      }

      else
      {
        this.WriteSystemMessage( "更改昵称失败，昵称已经被其他用户占用。" );
        return false;
      }
    }



    private PlayerConsole _console;

    /// <summary>
    /// 获取玩家控制台，用于给玩家显示消息
    /// </summary>
    public PlayerConsole Console
    {
      get { return _console; }
    }

    PlayerConsoleBase IPlayerHost.Console
    {
      get { return _console; }
    }



    /// <summary>
    /// 设置消息指针位置
    /// </summary>
    /// <param name="messageIndex"></param>
    internal void SetMessageIndex( int messageIndex ) { Console.SetMessageIndex( messageIndex ); }


    /// <summary>
    /// 最后一次收取的消息位置
    /// </summary>
    internal int LastMesageIndex { get { return Console.LastMesageIndex; } }

    /// <summary>
    /// 获取所有未收取的消息
    /// </summary>
    /// <returns></returns>
    public GameMessage[] GetMessages() { return Console.GetMessages(); }




    /// <summary>
    /// 若已经加入某个游戏，则获取游戏中的玩家对象
    /// </summary>
    public GamePlayerBase Player { get; private set; }


    GamePlayerBase IPlayerHost.GetPlayer()
    {
      return Player;
    }

    /// <summary>
    /// 通知玩家已经加入了某个游戏
    /// </summary>
    /// <param name="player"></param>
    public void JoinedGame( GamePlayerBase player )
    {

      lock ( globalSyncRoot )
      {
        if ( Player != null )
          throw new InvalidOperationException( "玩家当前已经在另一个游戏，无法加入游戏" );

        Player = player;
      }
    }


    /// <summary>
    /// 尝试退出某个游戏
    /// </summary>
    public bool TryQuitGame()
    {
      lock ( SyncRoot )
      {
        if ( Player == null )
          return false;

        Player.QuitGame();
        Player = null;
        return true;
      }
    }


    /// <summary>
    /// 释放玩家宿主所有资源，若玩家正在游戏，则强行退出。
    /// </summary>
    public void Release()
    {
      lock ( globalSyncRoot )
      {

        TryQuitGame();
        playerHosts.Remove( this );
        PlayerNameManager.RemoveName( this.Name );
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



    /// <summary>
    /// 重写 ToString 方法，输出玩家名称和编号
    /// </summary>
    /// <returns></returns>

    public override string ToString()
    {
      return string.Format( "{0}({1})", Name, Guid );
    }



    /// <summary>
    /// 获取客户端所支持的特性列表
    /// </summary>
    public string[] Supports
    {
      get { return null; }
    }
  }
}
