﻿using System;

namespace Ivony.TableGame.WebHost
{


  /// <summary>
  /// 玩家宿主，登陆用户在系统中的宿主对象
  /// </summary>
  public partial class PlayerHost : PlayerHostBase
  {


    internal PlayerHost( string name )
      : base( name )
    {
      Name = name;

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
      this.WriteSystemMessage( "欢迎您参与到通用桌面游戏引擎实例游戏项目！" );
      this.WriteSystemMessage( "通用桌面游戏引擎可以帮助您快速的构建您想象中的桌面游戏，其项目通过 Apache 2.0 协议开源。" );
      this.WriteSystemMessage( "项目地址： https://github.com/Ivony/TableGames" );

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
      QuitGame();
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
    /// 获取正在等待的响应
    /// </summary>
    internal IResponding Responding { get; set; }


    /// <summary>
    /// 如果有正在等待的响应，获取响应标识。
    /// </summary>
    public string RespondingID
    {
      get
      {
        if ( Responding == null )
          return null;

        else
          return Responding.RespondingID.ToString();
      }
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
        if ( Responding is OptionsResponding responding )
          return responding.Options;

        return null;

      }
    }




    /// <summary>
    /// 当玩家响应了消息时，调用此方法
    /// </summary>
    /// <param name="id">响应标识</param>
    /// <param name="message">响应的消息</param>
    internal void OnResponse( string message )
    {

      if ( Responding == null )
      {
        this.WriteSystemMessage( "响应已经失效" );
        return;
      }

      Responding.OnResponse( message );
    }



    /// <summary>
    /// 重写 ToString 方法，输出玩家名称和编号
    /// </summary>
    /// <returns></returns>

    public override string ToString()
    {
      return string.Format( "{0}({1})", Name, ID );
    }

  }
}
