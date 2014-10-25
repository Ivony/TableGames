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
  public partial class PlayerHost : PlayerHostBase
  {


    public Guid Guid
    {
      get;
      private set;
    }



    internal PlayerHost( Guid id, string name )
      : base( name )
    {
      Guid = id;
      Name = name;

      SyncRoot = new object();
      RefreshTime = DateTime.UtcNow;
      base.Console = new PlayerConsole( this );
    }



    /// <summary>
    /// 上次状态刷新时间
    /// </summary>
    public DateTime RefreshTime { get; private set; }

    internal void RefreshState()
    {
      RefreshTime = DateTime.UtcNow;
    }


    internal void ShowInitializeInfo()
    {
      this.WriteSystemMessage( "欢迎您参与到通用卡牌游戏引擎实例游戏项目，您在游戏中的昵称是 {0}，希望您能喜欢。", Name );
      this.WriteSystemMessage( "通用卡牌游戏引擎可以帮助您快速的构建您想象中的卡牌游戏，其项目通过 Apache 2.0 协议开源。" );
      this.WriteSystemMessage( "项目地址： https://github.com/Ivony/TableGames" );
      this.WriteSystemMessage( "特别感谢群友 @谁在秋千 对项目的大力支持和开发的 Web 游戏客户端。" );
      this.WriteSystemMessage( "记得点赞哦，，，，，，" );

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



    /// <summary>
    /// 覆写 Console 属性，返回强类型的玩家客户端对象
    /// </summary>
    protected new PlayerConsole Console
    {
      get { return (PlayerConsole) base.Console; }
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
    /// 释放玩家宿主所有资源，若玩家正在游戏，则强行退出。
    /// </summary>
    public void Release()
    {
      TryQuitGame();
      Players.Release( this );
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
    public Option[] GetOptions()
    {
      lock ( SyncRoot )
      {
        var responding = Responding as OptionsResponding;

        if ( responding == null )
          return null;

        return responding.Options;
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



    /// <summary>
    /// 获取一个值，指示客户端是否已经声明自己支持的特性列表
    /// </summary>
    public bool FeatureDeclared { get; private set; }

    /// <summary>
    /// 设置客户端支持的特性列表
    /// </summary>
    /// <param name="features"></param>
    internal void SetSupportFeatures( string[] features )
    {
      lock ( SyncRoot )
      {
        SupportFeatures.Clear();
        foreach ( var item in features )
          SupportFeatures.Add( item );


        FeatureDeclared = true;
      }
    }
  }
}
